namespace SourceCode.Creatures
{
    public class Enums
    {
        public class CreatureTemplateType
        {
            // change TestLizard to your lizard's name
            public static CreatureTemplate.Type TestLizard = new(nameof(TestLizard), true);
            public static CreatureTemplate.Type LizoBloing = new(nameof(LizoBloing), true);

            public void UnregisterValues()
            {
                if (TestLizard != null) TestLizard.Unregister(); TestLizard = null;
                if (LizoBloing != null) LizoBloing.Unregister(); LizoBloing = null;
            }
        }

        public class SandboxUnlockID
        {
            // same as above
            public static MultiplayerUnlocks.SandboxUnlockID TestLizard = new(nameof(TestLizard), true);
            public static MultiplayerUnlocks.SandboxUnlockID LizoBloing = new(nameof(LizoBloing), true);

            public void UnregisterValues()
            {
                if (TestLizard != null) TestLizard.Unregister(); TestLizard = null;
                if (LizoBloing != null) LizoBloing.Unregister(); LizoBloing = null;
            }
        }
    }
}