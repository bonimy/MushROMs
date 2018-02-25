// <copyright file="AreaObjectCode.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Smb1
{
    public enum AreaObjectCode
    {
        Invalid = -1,
        QuestionBlockMushroom,
        QuestionBlockCoin,
        HiddenBlockCoin,
        HiddenBlock1UP,
        BrickMushroom,
        BrickBeanstalk,
        BrickStar,
        Brick10Coins,
        Brick1UP,
        SidewaysPipe,
        UsedBlock,
        Trampoline,
        ReverseLPipe,
        FlagPole,
        Empty,
        Empty2,
        GreenIsland = 0x10 | MiscPlatformType.Trees,
        MushroomIsland = 0x10 | MiscPlatformType.Mushrooms,
        Cannon = 0x10 | MiscPlatformType.BulletBillTurrets,
        CloudGround = 0x10 | MiscPlatformType.CloudGround,

        HorizontalBricks = 0x20,
        HorizontalBlocks = 0x30,
        HorizontalCoins = 0x40,
        VerticalBricks = 0x50,
        VerticalBlocks = 0x60,
        UnenterablePipe = 0x70,
        EnterablePipe = 0x78,

        Hole = 0x0C00,
        BalanceHorizontalRope = 0x0C10,
        BridgeV7 = 0x0C20,
        BridgeV8 = 0x0C30,
        BridgeV10 = 0x0C40,
        HoleWithWater = 0x0C50,
        HorizontalQuestionBlocksV3 = 0x0C60,
        HorizontalQuestionBlocksV7 = 0x0C70,

        PageSkip = 0x0D00,
        AltReverseLPipe = 0x0D40,
        AltFlagPole,
        BowserAxe,
        RopeForAxe,
        BowserBridge,
        ScrollStopWarpZone,
        ScrollStop,
        AltScrollStop,
        RedCheepCheepFlying,
        BulletBillGenerator,
        StopContinuation,
        LoopCommand,

        BrickAndSceneryChange = 0x0E00,
        BackgroundChange = 0x0E40,

        RopeForLift = 0x0F00,
        LiftBalanceVerticalRope = 0x0F10,
        Castle = 0x0F20,
        Staircase = 0x0F30,
        LongReverseLPipe = 0x0F40,
        VerticalBalls = 0x0F50,
        Empty3 = 0x0F60,
        Empty4 = 0x0F70,
    }
}
