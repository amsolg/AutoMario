using BizHawk.Client.Common;
using BizHawk.Emulation.Cores.Nintendo.SNES;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BizHawk.Client.EmuHawk.tools.Ai.Super_Mario_World
{
	public static class Brain
	{
		private static RamSearch _ramSearch;
		private static List<Watch> _watchList;
		private static Dictionary<long, int> _currentWatches = new Dictionary<long, int>();
		private static Dictionary<long, int> _previousWatches = new Dictionary<long, int>();
		private static Dictionary<string, int> _neuralNetInputs = new Dictionary<string, int>();
		private static int _marioX;
		private static int _marioY;
		private static int _screenX;
		private static int _screenY;

		public static void Evaluate()
		{
			if (_ramSearch == null) Initialize();
			//UpdateInputs();
			//UpdateWeights();



			//List<Watch> previousWatches;
			//foreach (var watch in _currentWatchList)
			//{
			//	if (!_previousWatches[watch.Address].Equals(watch.Value)) _changedWatches.Add(watch.Address, watch.Value - _previousWatches[watch.Address]);
			//}
			//if (_changedWatches.Count() > 0)
			//{

			//}
			//foreach (var watch in _currentWatchList)
			//{
			//	_previousWatches[watch.Address] = watch.Value;
			//}
			//_changedWatches.Clear();

			//Mario.LevelX = _watchList.Single(w => w.Address == 0x94).Value;
			//Mario.LevelY = _watchList.Single(w => w.Address == 0x96).Value;
			//if (Mario.LevelX > 100)
			//{
			//	Global.ActiveController.PressButton("P1 Right");
			//	Global.ActiveController.ReleaseButton("P1 Left");
			//	if (Mario.LevelX % 100 == 0) Global.ActiveController.PressButton("P1 B");
			//}
		}

		public static void EvaluateInput(int input)
		{

		}

		private static void Initialize()
		{
			_ramSearch = GlobalWin.Tools.CustomLoad<RamSearch>(false);
			_watchList = _ramSearch.CustomStart(new List<int>()
			{
				0x100, // Game Mode.
					   // 00 Load Nintendo Presents 
					   // 01 Nintendo Presents 
					   // 02 Fade to Title Screen
					   // 03 Load Title Screen 
					   // 04 Prepare Title Screen 
					   // 05 Title Screen: Fade in
					   // 06 Title Screen: Circle effect 
					   // 07 Title Screen 
					   // 08 Title Screen: File select
					   // 09 Title Screen: File delete 
					   // 0A Title Screen: Player select 
					   // 0B Fade to Overworld
					   // 0C Load Overworld 
					   // 0D Overworld: Fade in 
					   // 0E Overworld
					   // 0F Fade to Level 
					   // 10 Fade to Level (black)
					   // 11 Load Level (Mario Start!)
					   // 12 Prepare Level 
					   // 13 Level: Fade in 
					   // 14 Level
					   // 15 Fade to Game Over 
					   // 16 Fade to Game Over? 
					   // 17 Game Over
					   // 18 Load Credits/Cutscene? 
					   // 19 Load Credits/Cutscene? 
					   // 1A Load Credits/Cutscene?
					   // 1B Ending: Credits / Cutscene 
					   // 1C Fade to Yoshi's House 
					   // 1D Fade to Yoshi's House (black)
					   // 1E Ending: Yoshi's House: Fade in 
					   // 1F Ending: Yoshi's House 
					   // 20 Fade to Enemies
					   // 21 Fade to Enemies (black)
					   // 22 Fade to Enemies? 
					   // 23 Fade to Enemies (black)?
					   // 24 Ending: Enemies: Fade in 
					   // 25 Ending: Enemies 
					   // 26 Fade to The End / Go to 22
					   // 27 Fade to The End (black) 
					   // 28 Ending: The End: Fade in 
					   // 29 Ending: The End

				0xDB2, // 2-Player Flag


				// Overworld

				0xDD5, // #$00 = Do not auto-walk while on the overworld. 
					   // #$01 = Level beaten, regular. 
					   // #$02 = Level beaten, secret. 
					   // #$80 = Exit level with Start+Select, by dying. 
					   // #$E0 = Level beaten (not for the first time), level is one of the tile numbers at $04:E5E6. 
					   // Do not move the player, but enable the save prompt.

				0x13C1, // Current overworld tile the player is standing on.

				0x13D9, // A pointer to various processes running on the overworld. 
						// #$01 = Activate overworld events. 
						// #$02 = Runs as soon as a level is beaten and the events have run. 
						// #$03 = Standing still on a level tile. 
						// #$04 = Player is moving in a certain direction. 
						// #$05 = Runs before settling on a level tile. 
						// #$06 = Fading out to #$07. 
						// #$07 = Switching between Mario and Luigi. 
						// #$08 = Fading in from #$07. 
						// #$09 = Follows up #$08, sets $7E:13D9 to #$03. 
						// #$0A = Switching between two submaps (not via warp pipe/star). 
						// #$0B = Activate star warp. 
						// #$0C = Player intro march (entering overworld for the first time). 

						// Also used in the level end march: 
						// #$00 = Show up the course clear text.
						// #$01 = Store bonus star text (if applicable), bonus stars not decrementing yet. 
						// #$02 = Count down timer/convert to score, add up bonus stars to total.
						// #$03 = Do nothing.

				0x1EA2, // Overworld level setting flags, location within the table corresponds to $7E:13BF. Format: bmesudlr.
						// b = level is beaten.
						// m = midway point has been passed.
						// e = no entry if level already passed.
						// s = open Save Prompt when level is beaten.
						// u = enable walking upwards.
						// d = enable walking downwards.
						// l = enable walking leftwards.
						// r = enable walking rightwards.
						// Setting the high bit of $7E:1EEB will enable the special stage features(autumn overworld palettes, etc.) in the ORIGINAL game.

				0x1F11, // Current submap for Mario. 
						// #$00 = Main map
						// #$01 = Yoshi's Island
						// #$02 = Vanilla Dome
						// #$03 = Forest of Illusion
						// #$04 = Valley of Bowser
						// #$05 = Special World
						// #$06 = Star World





				//Level

				0x19, // Player powerup/status. 
					  // #$00 = small; 
					  // #$01 = big; 
					  // #$02 = cape; 
					  // #$03 = fire.

				0x1A, // Layer 1 X position, current frame. Mirror of SNES register $210D.

				0x1C, // Layer 1 Y position, current frame. Mirror of SNES register $210E.

				0x72, // Player is in the air flag. 
					  // #$0B = Jumping/swimming upwards 
					  // #$0C = Shooting out of a slanted pipe, running at maximum speed - 
					  // #$24 = Descending/sinking.

				0x73, // Player is ducking flag. 
					  // #$00 = No; 
					  // #$04 = Yes.

				0x74, // Player is climbing flag. 
					  // #$00 = No; 
					  // #$1F = Yes.

				0x75, // Player is in water flag. 
					  // #$00 = No;.
					  // #$01 = Yes.

				0x76, // Player direction. 
					  // #$00 = Left.
					  // #$01 = Right.

				0x77, // Player blocked status - Used to check if player is blocked in a certain direction. 
					  // Format: SxxMUDLR  
					  // The M bit means that the player is in the middle of a block. 
					  // The S bit indicates that the player is touching the side of the screens while horizontal screen scrolling is disabled. 
					  // UDLR = up, down, left, right(contact with walls).

				0x7B, // Player X speed. 
					  // #$7F is the fastest rightwards speed, while #$80 is the highest leftwards. 
					  // +/-#$14 is fully walking, +/-#$2F is fully running. 
					  // #$01-#$7F is right 
					  // #$80-#$FF is left 
					  // #$00 is standing still.

				0x7D, // Player Y speed. 
					  // #$00-#$7F = falling.
					  // #$80-#$FF = rising. 
					  // #$80 is the highest upwards speed.
					  // #$7F is the highest downwards speed. 
					  // #$46 is the maximum fall speed.
					  // #$B3 is the normal jump speed.
					  // #$A4 is the jump speed when fully running.

				0x7E, // Player X position (16-bit), within the borders of the screen.

				0x80, // Player Y position (16-bit), within the borders of the screen.

				0x85, // Water level flag. 
					  // #$00 = No. 
					  // #$01 = Yes.

				0x86, // Slippery level flag. 
					  // #$00 = No. 
					  // #$01 through #$7F = Half-slippery. 
					  // #$80 through #$FF = Yes. 
					  // Possible values in the clean ROM are #$00 and #$80.

				0x90, // Player Y position within a block. Calculated with $7E:0096 & #$0F. Indicates whether the player is touching the top or the bottom of the block.

				0x92, // Player X position within a block. Calculated with $7E:0094 + #$08 & #$0F.

				0x93, // The side of a block the player is on. 
				      // It's set to #$00 for the right side and #$01 for the left side. This address is relative to the block the player is currently inside.

				0x94, // Player X position (16-bit) within the level.

				0x96, // Player Y position (16-bit) within the level.

				0x98, // Y position of block the player is touching, low byte. Also used in the creation of various sprite types/other blocks.

				0x99, // Y position of block the player is touching, high byte. Also used in the creation of various sprite types/other blocks.

				0x9A, // X position of block the player is touching, low byte. Also used in the creation of various sprite types/other blocks.

				0x9B, // X position of block the player is touching, high byte. Also used in the creation of various sprite types/other blocks.

				0xD1, // Player X position (16-bit) within the level, current frame (as opposed to $7E:0094).

				0xD3, // Player Y position (16-bit) within the level, current frame (as opposed to $7E:0096).

				0xD8, // Sprite Y position, low byte.

				0xE4, // Sprite X position, low byte.

				0xDBC, // Item in Mario's item box. 
					   // #$00 = Nothing. 
					   // #$01 = Mushroom; 
					   // #$02 = Cape; 
					   // #$03 = Fire Flower. 
					   // Note that you'll want $7E:0DC2 in most cases.

				0xF31, // Timer. 
					   // $7E:0F31 = Hundreds. 
					   // $7E:0F32 = Tens. 
					   // $7E:0F33 = Ones.

				0xF34, // Mario's score, divided by 10 (the last digit of the score counter is always zero).

				0xFAE, // Low byte of the angle of the Boo rings. 
					   // $7E:0FAE is for the first Boo ring active.
					   // $7E:0FAF is for the second Boo ring active.

				0xFB2, // Boo ring center X position, low byte. 
					   // $7E:0FB2 is for the first active Boo ring.
					   // $7E:0FB3 is for the second active Boo ring.

				0xFB6, // Boo ring center Y position, low byte. 
					   // $7E:0FB6 is for the first active Boo ring.
					   // $7E:0FB7 is for the second active Boo ring.

				0x13C7, // Yoshi color. 
						// #$04=yellow; 
						// #$06=blue; 
						// #$08=red; 
						// #$0A=green. Refreshes on level change.

				0x13C9, // Show "Continue/End" menu flag. 
						// #$00 = Don't show it; 
						// #$01 = freeze player, but don't load the text yet; 
						// #$02 = freeze player, load "Continue/End" menu.

						// Also used in the level end march: 
						// #$00 = Show up the course clear text.
						// #$01 = Store bonus star text (if applicable), bonus stars not decrementing yet. 
						// #$02 = Count down timer/convert to score, add up bonus stars to total.
						// #$03 = Do nothing.

				0x13E1, // Determines what kind of slope you are on. It's also set when flying with a cape. Possible values are:
						// #$00 - Not on any slope.
						// #$08 - Gradual slope left.
						// #$10 - Gradual slope right.
						// #$18 - Normal slope left.
						// #$20 - Normal slope right.
						// #$28 - Steep slope left.
						// #$30 - Steep slope right.
						// #$58 - Very steep slope left.
						// #$60 - Very steep slope right.
						// #$68 - Swooping down while flying and facing Left.
						// #$70 - Swooping down while flying and facing right.

				0x13E3, // Player is wall-running flag.
						// #$00 = not wall-running.
						// #$02 = on bottom left (45° angle).
						// #$03 = on bottom right (45° angle).
						// #$04 = on top left (45° angle, on top of the wall).
						// #$05 = on top right (45° angle, on top of the wall).
						// #$06 = on the left wall.
						// #$07 = on the right wall.
						// Note that custom block codes will not run while wall - running!

				0x13E4, // Player dash timer. 
						// Increments with #$02 every frame the player is walking on the ground with the dash button held, otherwise decrements until it is zero. 
						// #$70 indicates that the player is at its maximum running speed, and also means that the player is able to fly with a cape.

				0x13E8, //Cape spin interacts with sprites flag.
						// #$00 = don't interact. 
						// #$01 = do interact.

				0x13E9, // Cape interaction X position within the level. It's adjusted when the cape attack is used.

				0x13EB, // Cape interaction Y position within the level. It's adjusted when the cape attack is used.

				0x13EE, // What kind of slope the player is on.
						// #$FC = Very steep slope left.
						// #$FD = Steep slope left.
						// #$FE = Normal slope left.
						// #$FF = Gradual slope left.
						// #$00 = Not on slope.
						// #$01 = Gradual slope right.
						// #$02 = Normal slope right.
						// #$03 = Steep slope right.
						// #$04 = Very steep slope right.

				0x13EF, // Player is on ground flag.
						// Is only set to #$01 if touching the floor, and ignores touching sides, ceilings, and running up walls. 
						// Does not work correctly in blocks, as the value still has to be calculated at that point. 
						// Instead, the original value of $7E:13EF is stored over to $7E:008D.

				0x13F0, // Used to calculate the index to the direction the player faces while using a climbing net door. 
						// The formula is $7E:149D << 1 & #$0E | $7E:13F0. 
						// This address gets its value from $7E:13F9.

				0x13F1, // Vertical Scroll enable flag. 
						// #$00 = disabled. 
						// #$01 = enabled.

				0x13FA, // Whether the player is capable of jumping out of the water immediately or not (so just below the surface). 
						// #$00 = No. 
						// #$01 = Yes.

				0x13FC, // Currently active boss. Used for determining which graphics to load, 
						// as well as checking for various other purposes(like when the player should have priority over certain sprite backgrounds, 
						// that can be found in the boss rooms).
						// #$00 = Morton
						// #$01 = Roy
						// #$02 = Ludwig
						// #$03 = Bowser
						// #$04 = Reznor
						// Note that Iggy, Lemmy, Wendy and Larry don't make use of this. Cleared on level->overworld transitions, and possibly at other times too.

				0x1402, // A flag that is set when the player is on a note block that is currently going down, i.e. the bounce sprite is moving downwards. 
				        // If this wouldn't be set, the player would be pushed away from tile 152 (the tile that temporarily comes into place of the note block).

				0x1406, // This is set to #$80 if you bounce off of a springboard or a purple triangle (while on Yoshi, that is), and is cleared when touching the ground.
						// The game uses this, along with some other RAM addresses, to determine if the screen should scroll up with the player or not.

				0x1407, // Player gliding with cape phase.
						// #$00 = Not gliding.
						// #$01 = Gliding, rising.
						// #$02 = Gliding, staying on level.
						// #$03 = Gliding, sinking a little.
						// #$04 = Gliding, sinking more.
						// #$05 = Swooping down, initial phase.
						// #$06 = Swooping down, fast.
						// Controls player pose as well (table at $00:CE79).

				0x140D, // Spin Jump flag. 
						// #$00 = normal jump (or on ground). 
						// Any other value = spinjumping.

				0x1427, // Bowser clown car image.
						// 00-Regular
						// 01-Blinking
						// 02-Hurt
						// 03-Angry face
						// Higher values makes it cycle through the above ones.

				0x1470, // Carrying something flag. Very similar to $7E:148F, with the difference that, if this flag is set, 
						// the player's graphics don't change, and throw blocks can still be picked up as long as $7E:148F remains zero. 
						// If used in a carryable custom sprite to check if the player is already carrying something, 
						// definitely combine it with $7E:148F to assure there are no exceptions.
						// #$00 = Carrying nothing. 
						// #$01 = carrying something.

				0x1471, // Whether the player is on top of a solid sprite, and what kind of sprite that is.
						// #$01 = Standing on top of a floating rock, floating grass platform, floating skull, Mega Mole, carrot top lift, etc. 
						// This one calculates the player's position based on the next frame.
						// #$02 = Standing on top of a springboard, pea bouncer. 
						// This one calculates the player's position based on the next frame.
						// There's a check at $00:D60B so that the player can hold the jump button pressed for a longer while to jump higher.
						// #$03 = Standing on top of a brown chained platform, gray falling platform. 
						// This one calculates the player's position based on the current frame.

				0x1490, // Star power timer. Decrements every fourth frame, except when sprites are locked. The music will revert when this timer reaches #$1E.

				0x1491, // Amount of pixels on the X/Y axis a sprite has moved in the current frame. 
						// It is set after every call to update sprite position based on speed, 
						// and the routine that updates both X/Y position based on speed will leave $7E:1491 with the movement on the X axis in this address. 
						// Very often used for rideable sprites as this address can be added to the player position to move the player in tandem with the sprite.

				0x14BC, // Radius of the rotating brown platform, by default this value is #$50 (found at $01:CACC). 
						// This value is subtracted from $7E:14B4 and stored into $7E:14B0.
						// In SMW however, the value is always #$50. Additionally, the high byte is always #$00.
						// Note that this radius depends on sprite X position (which stays the same). 
						// It calculates the center position of the imaginary circle you rotate around from that position.
						// The further you increment the radius, the further you will have to move the sprite to the right in order for the center position to be the same.
						// See also $7E:14BF.

				0x14C8, // Sprite status table:
						// #$00 = Free slot, non-existent sprite.
						// #$01 = Initial phase of sprite.
						// #$02 = Killed, falling off screen.
						// #$03 = Smushed. Rex and shell-less Koopas can be in this state.
						// #$04 = Killed with a spinjump.
						// #$05 = Burning in lava; sinking in mud.
						// #$06 = Turn into coin at level end.
						// #$07 = Stay in Yoshi's mouth.
						// #$08 = Normal routine.
						// #$09 = Stationary / Carryable.
						// #$0A = Kicked.
						// #$0B = Carried.
						// #$0C = Powerup from being carried past goaltape.
						// States 08 and above are considered alive; sprites in other states are dead and should not be interacted with.

				0x14C9,
				0x14CA,
				0x14CB,
				0x14CC,
				0x14CD,
				0x14CE,
				0x14CF,
				0x14D0,
				0x14D1,
				0x14D2,
				0x14D3,

				0x14D4, // Sprite Y position, high byte.

				0x14E0, // Sprite X position, high byte.

				0x1594, // Miscellaneous sprite table. 
						// In classic Piranha Plants, it is used to check if the sprite should be made visible and have interaction with the player. 
						// If it's any non-zero value, that Piranha Plant will become invisible.

				0x15A0, // Sprite off screen flag, horizontal.

				0x15C4, // Sprite table which is set to #$01 if the sprite in question is off-screen. 
						// For example, the rings of the Ball 'n' Chain are never drawn if this is a non-zero value.

				0x15D0, // Sprite about to be eaten flag table. 
						// #$00 = No. 
						// #$01 = Yes.

				0x1632, // "Sprite is behind scenery" flag. Used by, among others, the net Koopas.

				0x164A, // Sprite is in water flag table. 
						// #$00 = Sprite not in water. 
						// #$01 = Sprite in water.
						// Used in the Roy battle as an indicator that the walls have to close in.

				0x1697, // Consecutive enemies stomped.

				0x1783, // Shooter number. 
						// #$00 = None 
						// #$01 = Bullet Bill shooter 
						// #$02 = Torpedo Launcher

				0x178B, // Shooter Y position, low byte.

				0x179B, // Shooter X position, low byte.

				0x186C, // Sprite off screen flag, vertical [TODO: This one uses multiple bits, see $01A3DF; figure out the details]

				0x187A, // Riding Yoshi Flag. #$00 = No, #$01 = Yes, #$02 = Yes, and turning around.

				0x18C2, // Player is inside Lakitu cloud flag.
						// #$00 = Not inside Lakitu cloud.
						// #$01 = Inside Lakitu cloud.
						// If the latter, the player does not animate as if he were walking or floating.

				0x18D2, // This address is incremented every time the player kills a sprite with a star while the star is active and will reset when the star runs out.
						// #$01 = 200 
						// #$02 = 400
						// #$03 = 800
						// #$04 = 1000
						// #$05 = 2000
						// #$06 = 4000
						// #$07 = 8000
						// #$08 and above = 1-Up

				0x18D4, // Red berries eaten by Yoshi. After 10 berries, the counter resets and Yoshi lays an egg, containing a mushroom.

				0x18D5, // Pink berries eaten by Yoshi. After 2 berries, the counter resets and Yoshi lays an egg, containing a coin game cloud.

				0x18D6, // Type of the current berry being eaten. 
						// #$00 = Coin (no effect except getting a coin), 
						// #$01 = Red, 
						// #$02 = Pink, 
						// #$03 = Green. 
						// Controls both color and what happens when Yoshi eats the berry.

				0x18DA, // Sprite number that spawns when Yoshi lays an egg. Valid values are #$74 (mushroom) and #$6A (coin game cloud).

				0x18E7, // Yoshi ground stomp flag. 
						// #$00 = Yoshi does not stomp the ground when landing on it. 
						// #$01 = Yoshi does stomp the ground when landing on it.
						// This is set to #$01 when a Yellow Yoshi has a shell in its mouth, or when any Yoshi has a yellow shell in its mouth.

				0x1905, // Iggy's/Larry's platform total number of tilts made counter. 
						// It will increment everytime the platform will be at a maximum tilt.
						// Only the lowest bit is ever used, and it controls which direction it should move.

				0x1907, // Iggy's platform rotation phase counter.
						// #$00 = First phase, tilting left.
						// #$01 = First phase, tilting right.
						// #$02 = Second phase, tilting left.
						// #$03 = Second phase, tilting right.
						// #$04 = Third phase, tilting left steeply.
						// #$05 = Third phase, tilting right steeply.
						// After the third phase ends, the counter resets.

				0x191C, // Indicates whether or not Yoshi has a key in his mouth. 
						// #$00 = no 
						// #$01 = yes

				0x1B80, // Player is on a climbing tile flag.
						// #$00 = Player is not on a climbing tile.
						// #$01 = Player is on a climbing tile.

				0x1B96, // Side exit enabled flag. 
						// #$00 = Disabled 
						// #$01 = enabled

				0x1B9F, // Number of broken tile pairs in the Reznor battle.
			});


			Global.ActiveController.ForceType(new SnesController().Definition);

			//_controllerInputs.AddRange(Global.ActiveController.Definition.BoolButtons.Select(b => Global.ActiveController.IsPressed(b) ? 1.0 : 0));
			//_titleScreenNeuralNetInputs.AddRange(_titleScreenWatchList.Select(v => (double)v.Value));
			//_titleScreenNeuralNet = new NeuralNet(new NeuronLayer[]{ new NeuronLayer(_controllerInputs.Count + _titleScreenNeuralNetInputs.Count, _controllerInputs.Count), new NeuronLayer(_titleScreenNeuralNetInputs.Count, 12) });

			//_currentNeuralNet = _titleScreenNeuralNet;
			//_currentNeuralNetInputs = _titleScreenNeuralNetInputs;
		}

		private static void getPositions()
		{
			_marioX = _watchList.Single(w => w.Address == 0x94).Value;
			_marioY = _watchList.Single(w => w.Address == 0x94).Value;

			var layer1x = _watchList.Single(w => w.Address == 0x1A).Value;
			var layer1y = _watchList.Single(w => w.Address == 0x1C).Value;

			_screenX = _marioX - layer1x;
			_screenY = _marioY - layer1y;
		}

		private static double getTile(double dx, double dy)
		{
			var x = Math.Floor((_marioX + dx + 8) / 16);
			var y = Math.Floor((_marioY + dy) / 16);
			
			return _ramSearch.CustomStart(new List<int>() { Convert.ToInt32(0x1C800 + Math.Floor(x / 0x10) * 0x1B0 + y * 0x10 + x % 0x10) }).Single().Value;
		}

		private static void getSprites()
		{
			var sprites = { };
			for (var slot = 0; slot < 12; slot++)
			{
				local status = memory.readbyte(0x14C8 + slot);
				if status ~= 0 then
					spritex = memory.readbyte(0xE4 + slot) + memory.readbyte(0x14E0 + slot) * 256;
				spritey = memory.readbyte(0xD8 + slot) + memory.readbyte(0x14D4 + slot) * 256;
				sprites[#sprites+1] = {["x"]=spritex, ["y"]=spritey};
					}
			return sprites;
		}

		private void getExtendedSprites()
		{
			local extended = { };
			for slot = 0, 11 do
					local number = memory.readbyte(0x170B + slot);
			if number ~= 0 then
				spritex = memory.readbyte(0x171F + slot) + memory.readbyte(0x1733 + slot) * 256;
			spritey = memory.readbyte(0x1715 + slot) + memory.readbyte(0x1729 + slot) * 256;
				extended[#extended+1] = {["x"]=spritex, ["y"]=spritey};
		
			return extended;
		}

		private void getInputs()
		{
			getPositions();

			sprites = getSprites();
			extended = getExtendedSprites();

			local inputs = { };

			for dy = -BoxRadius * 16, BoxRadius * 16, 16 do
					for dx = -BoxRadius * 16, BoxRadius * 16, 16 do
							inputs[#inputs+1] = 0;
			
			tile = getTile(dx, dy);
			if tile == 1 and _marioY +dy < 0x1B0 then
			   inputs[#inputs] = 1;
			end


			for i = 1,#sprites do
				distx = math.abs(sprites[i]["x"] - (_marioX + dx));
				disty = math.abs(sprites[i]["y"] - (_marioY + dy));
				if distx <= 8 and disty <= 8 then
					inputs[#inputs] = -1;
				end
			end

			for i = 1,#extended do
				distx = math.abs(extended[i]["x"] - (_marioX + dx));
				disty = math.abs(extended[i]["y"] - (_marioY + dy));
				if distx < 8 and disty< 8 then
				   inputs[#inputs] = -1;
			end


			return inputs;
		}
	}
}
