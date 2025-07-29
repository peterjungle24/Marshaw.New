using HUD;
using Menu.Remix;

namespace SourceCode.RemixMenu
{
    public static class RemixValues
    {
        // For future changing values
    }
    public class REMIX_menuses : OptionInterface
    {
        public static bool initialized; //if was initialized
        public static ManualLogSource logger { get => Plugin.logger; }

        public REMIX_menuses()
        {
            // for add configurables here
        }

        public override void Initialize()
        {
            try
            {
                // initialize the base
                base.Initialize();

                // create a new array of tabs
                Tabs = new OpTab[]
                {
                    // create a new OpTab with the class and name
                    
                    new OpTab(this, Translate("General") ),
                    new OpTab(this, Translate("Gameplay") ),
                    new OpTab(this, Translate("Marshaw") )
                    {
                        colorCanvas = Color.gray,
                        colorButton = Color.gray
                    },
                };

                // assign methods as variables
                var general = GeneralTab();
                var gameplay = GameplayTab();
                var marshaw = MarshawTab();

                // i use THE METHODS and add items to these tabs
                Tabs[0].AddItems(general);
                Tabs[1].AddItems(gameplay);
                Tabs[2].AddItems(marshaw);
            }
            catch (Exception eu)
            {
                Debug.Log($">: remix_menu.cs/Initialize :<");
                Debug.Log(eu);
            }
        }

        UIelement[] GeneralTab()
        {
            // create a array of contents inside
            UIelement[] tabArray = new UIelement[]
            {

            };

            return tabArray;
        }
        UIelement[] GameplayTab()
        {
            // create a array of contents inside
            UIelement[] tabArray = new UIelement[]
            {

            };

            return tabArray;
        }
        UIelement[] MarshawTab()
        {
            // create a array of contents inside
            UIelement[] tabArray = new UIelement[]
            {

            };

            return tabArray;
        }

    }
}