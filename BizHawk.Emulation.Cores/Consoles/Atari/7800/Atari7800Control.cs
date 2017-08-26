﻿using System;

using EMU7800.Core;
using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.Atari.Atari7800
{
	public class Atari7800Control
	{
		private static readonly ControllerDefinition Joystick = new ControllerDefinition
		{
			Name = "Atari 7800 Joystick Controller",
			BoolButtons =
			{
				// hard reset, not passed to EMU7800
				"Power",

				// on the console
				"Reset",
				"Select",
				"BW", // should be "Color"??
				"Left Difficulty", // better not put P# on these as they might not correspond to player numbers
				"Right Difficulty",

				// ports
				"P1 Up", "P1 Down", "P1 Left", "P1 Right", "P1 Trigger",
				"P2 Up", "P2 Down", "P2 Left", "P2 Right", "P2 Trigger"
			}
		};

		private static readonly ControllerDefinition Paddles = new ControllerDefinition
		{
			Name = "Atari 7800 Paddle Controller",
			BoolButtons = 
			{
				// hard reset, not passed to EMU7800
				"Power",

				// on the console
				"Reset",
				"Select",
				"BW", // should be "Color"??
				"Left Difficulty", // better not put P# on these as they might not correspond to player numbers
				"Right Difficulty",

				// ports
				"P1 Trigger",
				"P2 Trigger",
				"P3 Trigger",
				"P4 Trigger"
			},
			FloatControls = // should be in [0..700000]
			{
				"P1 Paddle",
				"P2 Paddle",
				"P3 Paddle",
				"P4 Paddle"
			},
			FloatRanges =
			{
				// what is the center point supposed to be here?
				new[] { 0.0f, 0.0f, 700000.0f },
				new[] { 0.0f, 0.0f, 700000.0f },
				new[] { 0.0f, 0.0f, 700000.0f },
				new[] { 0.0f, 0.0f, 700000.0f }
			}
		};

		private static readonly ControllerDefinition Keypad = new ControllerDefinition
		{
			Name = "Atari 7800 Keypad Controller",
			BoolButtons = 
			{
				// hard reset, not passed to EMU7800
				"Power",

				// on the console
				"Reset",
				"Select",
				"BW", // should be "Color"??
				"Toggle Left Difficulty", // better not put P# on these as they might not correspond to player numbers
				"Toggle Right Difficulty",

				// ports
				"P1 Keypad1", "P1 Keypad2", "P1 Keypad3", 
				"P1 Keypad4", "P1 Keypad5", "P1 Keypad6", 
				"P1 Keypad7", "P1 Keypad8", "P1 Keypad9", 
				"P1 KeypadA", "P1 Keypad0", "P1 KeypadP", 
				"P2 Keypad1", "P2 Keypad2", "P2 Keypad3", 
				"P2 Keypad4", "P2 Keypad5", "P2 Keypad6", 
				"P2 Keypad7", "P2 Keypad8", "P2 Keypad9", 
				"P2 KeypadA", "P2 Keypad0", "P2 KeypadP", 
				"P3 Keypad1", "P3 Keypad2", "P3 Keypad3", 
				"P3 Keypad4", "P3 Keypad5", "P3 Keypad6", 
				"P3 Keypad7", "P3 Keypad8", "P3 Keypad9", 
				"P3 KeypadA", "P3 Keypad0", "P3 KeypadP", 
				"P4 Keypad1", "P4 Keypad2", "P4 Keypad3", 
				"P4 Keypad4", "P4 Keypad5", "P4 Keypad6", 
				"P4 Keypad7", "P4 Keypad8", "P4 Keypad9", 
				"P4 KeypadA", "P4 Keypad0", "P4 KeypadP"
			}
		};

		private static readonly ControllerDefinition Driving = new ControllerDefinition
		{
			Name = "Atari 7800 Driving Controller",
			BoolButtons = 
			{
				// hard reset, not passed to EMU7800
				"Power",

				// on the console
				"Reset",
				"Select",
				"BW", // should be "Color"??
				"Toggle Left Difficulty", // better not put P# on these as they might not correspond to player numbers
				"Toggle Right Difficulty",

				// ports
				"P1 Trigger",
				"P2 Trigger"
			},
			FloatControls = // should be in [0..3]
			{
				"P1 Driving",
				"P2 Driving"
			},
			FloatRanges =
			{
				new[] { 0.0f, 0.0f, 3.0f },
				new[] { 0.0f, 0.0f, 3.0f },
				new[] { 0.0f, 0.0f, 3.0f }
			}
		};

		private static readonly ControllerDefinition BoosterGrip = new ControllerDefinition
		{
			Name = "Atari 7800 Booster Grip Controller",
			BoolButtons = 
			{
				// hard reset, not passed to EMU7800
				"Power",

				// on the console
				"Reset",
				"Select",
				"BW", // should be "Color"??
				"Toggle Left Difficulty", // better not put P# on these as they might not correspond to player numbers
				"Toggle Right Difficulty",

				// ports
				// NB: as referenced by the emu, p1t2 = p1t2, p1t3 = p2t2, p2t2 = p3t2, p2t3 = p4t2
				"P1 Up", "P1 Down", "P1 Left", "P1 Right", "P1 Trigger", "P1 Trigger 2", "P1 Trigger 3",
				"P2 Up", "P2 Down", "P2 Left", "P2 Right", "P2 Trigger", "P2 Trigger 2", "P2 Trigger 3"
			}
		};

		private static readonly ControllerDefinition ProLineJoystick = new ControllerDefinition
		{
			Name = "Atari 7800 ProLine Joystick Controller",
			BoolButtons =
			{
				// hard reset, not passed to EMU7800
				"Power",

				// on the console
				"Reset",
				"Select",
				"Pause",
				"Toggle Left Difficulty", // better not put P# on these as they might not correspond to player numbers
				"Toggle Right Difficulty",

				// ports
				"P1 Up", "P1 Down", "P1 Left", "P1 Right", "P1 Trigger", "P1 Trigger 2",
				"P2 Up", "P2 Down", "P2 Left", "P2 Right", "P2 Trigger", "P2 Trigger 2"
			}
		};

		private static readonly ControllerDefinition Lightgun = new ControllerDefinition
		{
			Name = "Atari 7800 Light Gun Controller",
			BoolButtons =
			{
				// hard reset, not passed to EMU7800
				"Power",

				// on the console
				"Reset",
				"Select",
				"Pause",
				"Left Difficulty", // better not put P# on these as they might not correspond to player numbers
				"Right Difficulty",

				// ports
				"P1 Trigger",
				"P2 Trigger"
			},
			FloatControls = // vpos should be actual scanline number.  hpos should be in [0..319]??
			{
				"P1 VPos", "P1 HPos",
				"P2 VPos", "P2 HPos"
			},
			FloatRanges =
			{
				// how many scanlines are there again??
				new[] { 0.0f, 0.0f, 240.0f },
				new[] { 0.0f, 0.0f, 319.0f },
				new[] { 0.0f, 0.0f, 240.0f },
				new[] { 0.0f, 0.0f, 319.0f }
			}
		};

		private struct ControlAdapter
		{
			public readonly ControllerDefinition Type;
			public readonly Controller Left;
			public readonly Controller Right;
			public readonly Action<IController, InputState> Convert;

			public ControlAdapter(ControllerDefinition type, Controller left, Controller right, Action<IController, InputState> convert)
			{
				Type = type;
				Left = left;
				Right = right;
				Convert = convert;
			}
		}

		private static readonly ControlAdapter[] Adapters =
		{
			new ControlAdapter(Joystick, Controller.Joystick, Controller.Joystick, ConvertJoystick),
			new ControlAdapter(Paddles, Controller.Paddles, Controller.Paddles, ConvertPaddles),
			new ControlAdapter(Keypad, Controller.Keypad, Controller.Keypad, ConvertKeypad),
			new ControlAdapter(Driving, Controller.Driving, Controller.Driving, ConvertDriving),
			new ControlAdapter(BoosterGrip, Controller.BoosterGrip, Controller.BoosterGrip, ConvertBoosterGrip),
			new ControlAdapter(ProLineJoystick, Controller.ProLineJoystick, Controller.ProLineJoystick, ConvertProLineJoystick),
			new ControlAdapter(Lightgun, Controller.Lightgun, Controller.Lightgun, ConvertLightgun),
		};

		private static void ConvertConsoleButtons(IController c, InputState s)
		{
			s.RaiseInput(0, MachineInput.Reset, c.IsPressed("Reset"));
			s.RaiseInput(0, MachineInput.Select, c.IsPressed("Select"));
			s.RaiseInput(0, MachineInput.Color, c.IsPressed("BW"));
			if (c.IsPressed("Toggle Left Difficulty"))
			{
				s.RaiseInput(0, MachineInput.LeftDifficulty, c.IsPressed("Toggle Left Difficulty"));
			}

			if (c.IsPressed("Toggle Right Difficulty"))
			{
				s.RaiseInput(0, MachineInput.RightDifficulty, c.IsPressed("Toggle Right Difficulty"));
			}
		}

		private static void ConvertConsoleButtons7800(IController c, InputState s)
		{
			s.RaiseInput(0, MachineInput.Reset, c.IsPressed("Reset"));
			s.RaiseInput(0, MachineInput.Select, c.IsPressed("Select"));
			s.RaiseInput(0, MachineInput.Color, c.IsPressed("Pause"));
			if (c.IsPressed("Toggle Left Difficulty"))
			{
				s.RaiseInput(0, MachineInput.LeftDifficulty, c.IsPressed("Toggle Left Difficulty"));
			}

			if (c.IsPressed("Toggle Right Difficulty"))
			{
				s.RaiseInput(0, MachineInput.RightDifficulty, c.IsPressed("Toggle Right Difficulty"));
			}
		}

		private static void ConvertDirections(IController c, InputState s, int p)
		{
			string ps = $"P{p + 1} ";
			s.RaiseInput(p, MachineInput.Up, c.IsPressed(ps + "Up"));
			s.RaiseInput(p, MachineInput.Down, c.IsPressed(ps + "Down"));
			s.RaiseInput(p, MachineInput.Left, c.IsPressed(ps + "Left"));
			s.RaiseInput(p, MachineInput.Right, c.IsPressed(ps + "Right"));
		}

		private static void ConvertTrigger(IController c, InputState s, int p)
		{
			string ps = $"P{p + 1} ";
			s.RaiseInput(p, MachineInput.Fire, c.IsPressed(ps + "Trigger"));
		}

		private static void ConvertJoystick(IController c, InputState s)
		{
			s.ClearControllerInput();
			ConvertConsoleButtons(c, s);
			ConvertDirections(c, s, 0);
			ConvertDirections(c, s, 1);
			ConvertTrigger(c, s, 0);
			ConvertTrigger(c, s, 1);
		}

		private static void ConvertPaddles(IController c, InputState s)
		{
			s.ClearControllerInput();
			ConvertConsoleButtons(c, s);
			for (int i = 0; i < 4; i++)
			{
				string ps = $"P{i + 1} ";
				ConvertTrigger(c, s, i);
				s.RaisePaddleInput(i, 700000, (int)c.GetFloat(ps + "Trigger"));
			}
		}

		private static void ConvertKeypad(IController c, InputState s)
		{
			s.ClearControllerInput();
			ConvertConsoleButtons(c, s);
			for (int i = 0; i < 4; i++)
			{
				string ps = $"P{i + 1} ";
				s.RaiseInput(i, MachineInput.NumPad1, c.IsPressed(ps + "Keypad1"));
				s.RaiseInput(i, MachineInput.NumPad2, c.IsPressed(ps + "Keypad2"));
				s.RaiseInput(i, MachineInput.NumPad3, c.IsPressed(ps + "Keypad3"));
				s.RaiseInput(i, MachineInput.NumPad4, c.IsPressed(ps + "Keypad4"));
				s.RaiseInput(i, MachineInput.NumPad5, c.IsPressed(ps + "Keypad5"));
				s.RaiseInput(i, MachineInput.NumPad6, c.IsPressed(ps + "Keypad6"));
				s.RaiseInput(i, MachineInput.NumPad7, c.IsPressed(ps + "Keypad7"));
				s.RaiseInput(i, MachineInput.NumPad8, c.IsPressed(ps + "Keypad8"));
				s.RaiseInput(i, MachineInput.NumPad9, c.IsPressed(ps + "Keypad9"));
				s.RaiseInput(i, MachineInput.NumPadMult, c.IsPressed(ps + "KeypadA"));
				s.RaiseInput(i, MachineInput.NumPad0, c.IsPressed(ps + "Keypad0"));
				s.RaiseInput(i, MachineInput.NumPadHash, c.IsPressed(ps + "KeypadP"));
			}
		}

		private static readonly MachineInput[] Drvlut =
		{
			MachineInput.Driving0,
			MachineInput.Driving1,
			MachineInput.Driving2,
			MachineInput.Driving3
		};

		private static void ConvertDriving(IController c, InputState s)
		{
			s.ClearControllerInput();
			ConvertConsoleButtons(c, s);
			ConvertTrigger(c, s, 0);
			ConvertTrigger(c, s, 1);
			s.RaiseInput(0, Drvlut[(int)c.GetFloat("P1 Driving")], true);
			s.RaiseInput(1, Drvlut[(int)c.GetFloat("P2 Driving")], true);
		}

		private static void ConvertBoosterGrip(IController c, InputState s)
		{
			s.ClearControllerInput();
			ConvertConsoleButtons(c, s);
			ConvertDirections(c, s, 0);
			ConvertDirections(c, s, 1);

			// weird mapping is intentional
			s.RaiseInput(0, MachineInput.Fire, c.IsPressed("P1 Trigger"));
			s.RaiseInput(0, MachineInput.Fire2, c.IsPressed("P1 Trigger 2"));
			s.RaiseInput(1, MachineInput.Fire2, c.IsPressed("P1 Trigger 3"));
			s.RaiseInput(1, MachineInput.Fire, c.IsPressed("P2 Trigger"));
			s.RaiseInput(2, MachineInput.Fire2, c.IsPressed("P2 Trigger 2"));
			s.RaiseInput(3, MachineInput.Fire2, c.IsPressed("P2 Trigger 3"));
		}

		private static void ConvertProLineJoystick(IController c, InputState s)
		{
			s.ClearControllerInput();
			ConvertConsoleButtons7800(c, s);
			ConvertDirections(c, s, 0);
			ConvertDirections(c, s, 1);
			s.RaiseInput(0, MachineInput.Fire, c.IsPressed("P1 Trigger"));
			s.RaiseInput(0, MachineInput.Fire2, c.IsPressed("P1 Trigger 2"));
			s.RaiseInput(1, MachineInput.Fire, c.IsPressed("P2 Trigger"));
			s.RaiseInput(1, MachineInput.Fire2, c.IsPressed("P2 Trigger 2"));
		}

		private static void ConvertLightgun(IController c, InputState s)
		{
			s.ClearControllerInput();
			ConvertConsoleButtons7800(c, s);
			ConvertTrigger(c, s, 0);
			ConvertTrigger(c, s, 1);
			s.RaiseLightgunPos(0, (int)c.GetFloat("P1 VPos"), (int)c.GetFloat("P1 HPos"));
			s.RaiseLightgunPos(1, (int)c.GetFloat("P2 VPos"), (int)c.GetFloat("P2 HPos"));
		}

		public Action<IController, InputState> Convert { get; private set; }

		public ControllerDefinition ControlType { get; private set; }

		public Atari7800Control(MachineBase mac)
		{
			var l = mac.InputState.LeftControllerJack;
			var r = mac.InputState.RightControllerJack;

			foreach (var a in Adapters)
			{
				if (a.Left == l && a.Right == r)
				{
					Convert = a.Convert;
					ControlType = a.Type;
					return;
				}
			}

			throw new Exception($"Couldn't connect Atari 7800 controls \"{l}\" and \"{r}\"");
		}
	}
}
