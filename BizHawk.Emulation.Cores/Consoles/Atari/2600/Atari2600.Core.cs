﻿using System;

using BizHawk.Common.NumberExtensions;
using BizHawk.Common.BufferExtensions;

using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Components.M6502;

namespace BizHawk.Emulation.Cores.Atari.Atari2600
{
	public partial class Atari2600
	{
		private readonly GameInfo _game;

		private TIA _tia;
		private M6532 _m6532;
		private DCFilter _dcfilter;
		private MapperBase _mapper;
		private byte[] _ram;

		private IController _controller;
		private int _frame;
		private int _lastAddress;
		private bool _frameStartPending = true;

		private bool _leftDifficultySwitchPressed;
		private bool _rightDifficultySwitchPressed;

		private bool _leftDifficultySwitchHeld;
		private bool _rightDifficultySwitchHeld;

		internal MOS6502X Cpu { get; private set; }
		internal byte[] Ram => _ram;
		internal byte[] Rom { get; }
		internal int DistinctAccessCount { get; private set; }

		private static MapperBase SetMultiCartMapper(int romLength, int gameTotal)
		{
			switch (romLength / gameTotal)
			{
				case 1024 * 2: // 2K
					return new Multicart2K(gameTotal);
				default:
				case 1024 * 4: // 4K
					return new Multicart4K(gameTotal);
				case 1024 * 8: // 8K
					return new Multicart8K(gameTotal);
			}
		}

		internal byte BaseReadMemory(ushort addr)
		{
			addr = (ushort)(addr & 0x1FFF);
			if ((addr & 0x1080) == 0)
			{
				return _tia.ReadMemory(addr, false);
			}

			if ((addr & 0x1080) == 0x0080)
			{
				_tia.BusState = _m6532.ReadMemory(addr, false);
				return _m6532.ReadMemory(addr, false);
			}

			_tia.BusState = Rom[addr & 0x0FFF];
			return Rom[addr & 0x0FFF];
		}

		internal byte BasePeekMemory(ushort addr)
		{
			addr = (ushort)(addr & 0x1FFF);
			if ((addr & 0x1080) == 0)
			{
				return _tia.ReadMemory(addr, true);
			}

			if ((addr & 0x1080) == 0x0080)
			{
				return _m6532.ReadMemory(addr, true);
			}

			return Rom[addr & 0x0FFF];
		}

		internal void BaseWriteMemory(ushort addr, byte value)
		{
			_tia.BusState = value;
			if (addr != _lastAddress)
			{
				DistinctAccessCount++;
				_lastAddress = addr;
			}

			addr = (ushort)(addr & 0x1FFF);
			if ((addr & 0x1080) == 0)
			{
				_tia.WriteMemory(addr, value, false);
			}
			else if ((addr & 0x1080) == 0x0080)
			{
				_m6532.WriteMemory(addr, value);
			}
			else
			{
				Console.WriteLine("ROM write(?):  " + addr.ToString("x"));
			}
		}

		internal void BasePokeMemory(ushort addr, byte value)
		{
			addr = (ushort)(addr & 0x1FFF);
			if ((addr & 0x1080) == 0)
			{
				_tia.WriteMemory(addr, value, true);
			}
			else if ((addr & 0x1080) == 0x0080)
			{
				_m6532.WriteMemory(addr, value);
			}
			else
			{
				Console.WriteLine("ROM write(?):  " + addr.ToString("x"));
			}
		}

		private byte ReadMemory(ushort addr)
		{
			if (addr != _lastAddress)
			{
				DistinctAccessCount++;
				_lastAddress = addr;
			}

			_mapper.Bit13 = addr.Bit(13);
			var temp = _mapper.ReadMemory((ushort)(addr & 0x1FFF));
			_tia.BusState = temp;
			MemoryCallbacks.CallReads(addr);

			return temp;
		}

		private byte PeekMemory(ushort addr)
		{
			var temp = _mapper.PeekMemory((ushort)(addr & 0x1FFF));
			return temp;
		}

		private void WriteMemory(ushort addr, byte value)
		{
			if (addr != _lastAddress)
			{
				DistinctAccessCount++;
				_lastAddress = addr;
			}

			_mapper.WriteMemory((ushort)(addr & 0x1FFF), value);

			MemoryCallbacks.CallWrites(addr);
		}

		internal void PokeMemory(ushort addr, byte value)
		{
			_mapper.PokeMemory((ushort)(addr & 0x1FFF), value);
		}

		private void ExecFetch(ushort addr)
		{
			MemoryCallbacks.CallExecutes(addr);
		}

		private void RebootCore()
		{
			// Regenerate mapper here to make sure its state is entirely clean
			switch (_game.GetOptionsDict()["m"])
			{
				case "2IN1":
					_mapper = SetMultiCartMapper(Rom.Length, 2);
					break;
				case "4IN1":
					_mapper = SetMultiCartMapper(Rom.Length, 4);
					break;
				case "8IN1":
					_mapper = SetMultiCartMapper(Rom.Length, 8);
					break;
				case "16IN1":
					_mapper = SetMultiCartMapper(Rom.Length, 16);
					break;
				case "32IN1":
					_mapper = SetMultiCartMapper(Rom.Length, 32);
					break;
				case "AR":
					_mapper = new mAR(this); // This mapper has to set up configurations in the contructor.
					break;
				case "4K":
					_mapper = new m4K();
					break;
				case "2K":
					_mapper = new m2K();
					break;
				case "CM":
					_mapper = new mCM();
					break;
				case "CV":
					_mapper = new mCV();
					break;
				case "DPC":
					_mapper = new mDPC();
					break;
				case "DPC+":
					_mapper = new mDPCPlus();
					break;
				case "F8":
					_mapper = new mF8();
					break;
				case "F8SC":
					_mapper = new mF8SC();
					break;
				case "F6":
					_mapper = new mF6();
					break;
				case "F6SC":
					_mapper = new mF6SC();
					break;
				case "F4":
					_mapper = new mF4();
					break;
				case "F4SC":
					_mapper = new mF4SC();
					break;
				case "FE":
					_mapper = new mFE();
					break;
				case "E0":
					_mapper = new mE0();
					break;
				case "3F":
					_mapper = new m3F();
					break;
				case "FA":
					_mapper = new mFA();
					break;
				case "FA2":
					_mapper = new mFA2();
					break;
				case "E7":
					_mapper = new mE7();
					break;
				case "F0":
					_mapper = new mF0();
					break;
				case "UA":
					_mapper = new mUA();
					break;

				// Special Sega Mapper which has swapped banks
				case "F8_sega":
					_mapper = new mF8_sega();
					break;

				// Homebrew mappers
				case "3E":
					_mapper = new m3E();
					break;
				case "0840":
					_mapper = new m0840();
					break;
				case "MC":
					_mapper = new mMC();
					break;
				case "EF":
					_mapper = new mEF();
					break;
				case "EFSC":
					_mapper = new mEFSC();
					break;
				case "X07":
					_mapper = new mX07();
					break;
				case "4A50":
					_mapper = new m4A50();
					break;
				case "SB":
					_mapper = new mSB();
					break;

				default:
					throw new InvalidOperationException("mapper not supported: " + _game.GetOptionsDict()["m"]);
			}

			_mapper.Core = this;

			_lagcount = 0;
			Cpu = new MOS6502X
			{
				ReadMemory = ReadMemory,
				WriteMemory = WriteMemory,
				PeekMemory = PeekMemory,
				DummyReadMemory = ReadMemory,
				OnExecFetch = ExecFetch
			};

			if (_game["PAL"])
			{
				_pal = true;
			}
			else if (_game["NTSC"])
			{
				_pal = false;
			}
			else
			{
				_pal = DetectPal(_game, Rom);
			}

			// dcfilter coefficent is from real observed hardware behavior: a latched "1" will fully decay by ~170 or so tia sound cycles
			_tia = new TIA(this, _pal, Settings.SECAMColors);

			_dcfilter = new DCFilter(_tia, 256);

			_m6532 = new M6532(this);

			// Set up the system state here. for instance..
			// Read from the reset vector for where to start
			Cpu.PC = (ushort)(ReadMemory(0x1FFC) + (ReadMemory(0x1FFD) << 8)); // set the initial PC

			// Show mapper class on romstatusdetails
			CoreComm.RomStatusDetails = $"{this._game.Name}\r\nSHA1:{Rom.HashSHA1()}\r\nMD5:{Rom.HashMD5()}\r\nMapper Impl \"{_mapper.GetType()}\"";

			// as it turns out, the stack pointer cannot be set to 0 for some games as they do not initilize it themselves. 
			// some documentation seems to indicate it should beset to FD, but currently there is no documentation of the 6532 
			// executing a reset sequence at power on, but it's needed so let's hard code it for now
			Cpu.S = 0xFD;
		}

		private bool _pal;

		private void HardReset()
		{
			_ram = new byte[128];
			_mapper.HardReset();

			Cpu = new MOS6502X
			{
				ReadMemory = ReadMemory,
				WriteMemory = WriteMemory,
				PeekMemory = PeekMemory,
				DummyReadMemory = ReadMemory,
				OnExecFetch = ExecFetch
			};

			_tia.Reset();

			_m6532 = new M6532(this);
			Cpu.PC = (ushort)(ReadMemory(0x1FFC) + (ReadMemory(0x1FFD) << 8)); // set the initial PC

			// as it turns out, the stack pointer cannot be set to 0 for some games as they do not initilize it themselves. 
			// some documentation seems to indicate it should beset to FD, but currently there is no documentation of the 6532 
			// executing a reset sequence at power on, but it's needed so let's hard code it for now
			Cpu.S = 0xFD;
			
			SetupMemoryDomains();
		}

		private void VFrameAdvance() // advance up to 500 lines looking for end of video frame
			// after vsync falling edge, continues to end of next line
		{
			bool frameend = false;
			_tia.FrameEndCallBack = (n) => frameend = true;
			for (int i = 0; i < 500 && !frameend; i++)
			{
				ScanlineAdvance();
			}

			_tia.FrameEndCallBack = null;
		}

		private void StartFrameCond()
		{
			if (_frameStartPending)
			{
				_frame++;
				_islag = true;

				if (_controller.IsPressed("Power"))
				{
					HardReset();
				}

				if (_controller.IsPressed("Toggle Left Difficulty") && !_leftDifficultySwitchHeld)
				{
					_leftDifficultySwitchPressed ^= true;
					_leftDifficultySwitchHeld = true;
				}
				else if (!_controller.IsPressed("Toggle Left Difficulty"))
				{
					_leftDifficultySwitchHeld = false;
				}

				if (_controller.IsPressed("Toggle Right Difficulty") && !_rightDifficultySwitchHeld)
				{
					_rightDifficultySwitchPressed ^= true;
					_rightDifficultySwitchHeld = true;
				}
				else if (!_controller.IsPressed("Toggle Right Difficulty"))
				{
					_rightDifficultySwitchHeld = false;
				}

				_tia.BeginAudioFrame();
				_frameStartPending = false;
			}
		}

		private void FinishFrameCond()
		{
			if (_tia.LineCount >= _tia.NominalNumScanlines)
			{
				_tia.CompleteAudioFrame();
				if (_islag)
				{
					_lagcount++;
				}

				_tia.LineCount = 0;
				_frameStartPending = true;
			}
		}

		private void Cycle()
		{
			_tia.Execute(1);
			_tia.Execute(1);
			_tia.Execute(1);
			_m6532.Timer.Tick();
			if (Tracer.Enabled)
			{
				Tracer.Put(Cpu.TraceState());
			}

			Cpu.ExecuteOne();
			_mapper.ClockCpu();
		}

		internal byte ReadControls1(bool peek)
		{
			InputCallbacks.Call();
			
			byte value = _controllerDeck.ReadPort1(_controller);

			if (!peek)
			{
				_islag = false;
			}

			return value;
		}

		internal byte ReadControls2(bool peek)
		{
			InputCallbacks.Call();
			byte value = _controllerDeck.ReadPort2(_controller);

			if (!peek)
			{
				_islag = false;
			}

			return value;
		}

		internal int ReadPot1(int pot)
		{
			int value = _controllerDeck.ReadPot1(_controller, pot);

			return value;
		}

		internal int ReadPot2(int pot)
		{
			int value = _controllerDeck.ReadPot2(_controller, pot);

			return value;
		}

		internal byte ReadConsoleSwitches(bool peek)
		{
			byte value = 0xFF;
			bool select = _controller.IsPressed("Select");
			bool reset = _controller.IsPressed("Reset");

			if (reset) { value &= 0xFE; }
			if (select) { value &= 0xFD; }
			if (SyncSettings.BW) { value &= 0xF7; }
			if (_leftDifficultySwitchPressed)
			{
				value &= 0xBF;
			}

			if (_rightDifficultySwitchPressed)
			{
				value &= 0x7F;
			}

			if (!peek)
			{
				_islag = false;
			}

			return value;
		}
	}
}
