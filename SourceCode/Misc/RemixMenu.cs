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
        public static LogUtils.Logger logger { get => Plugin.log; }

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
                UnityEngine.Debug.Log($">: remix_menu.cs/Initialize :<");
                UnityEngine.Debug.Log(eu);
            }
        }

        UIelement[] GeneralTab()
        {
            // - Make a button
            OpSimpleButton[] simpleButtons = new OpSimpleButton[]
            {
                new OpSimpleButton(new(200, 50), new(100, 25), "Hello guys!")
            };

            // create a array of contents inside
            UIelement[] tabArray = new UIelement[]
            {
                // - Add a button
                simpleButtons[0]
            };

            // Set a method event
            simpleButtons[0].OnClick += SomeDialogTest;

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

        void SomeDialogTest(UIfocusable trigger)
        {
            // - Cool for shortened "new line" (i am just lazy atp)
            var nl = Environment.NewLine;

            // - The dialog that will appear in the Dialog Box
            string[] dialog = new string[]
            {
                Translate("Hello!"), nl,
                Translate("This is just a testing dialog box."), nl,
                Translate("Dont worry, thats normal.")
            };
            // - Make it as a String
            string res = string.Concat(dialog);

            // - Dialog text, and the method that will be called in YES option
            //  - the NO option its also avaible but optional.
            ConfigConnector.CreateDialogBoxYesNo(res, YesAction);
        }
        void YesAction()
        {
            // - Cool for shortened "new line" (i am just lazy atp)
            var nl = Environment.NewLine;

            // - The dialog that will appear in the Dialog Box
            string[] dialog = new string[]
            {
                Translate("_¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶"), nl,
                Translate("_¶¶___________________________________¶¶"), nl,
                Translate("_¶¶___________________________________¶¶"), nl,
                Translate("__¶¶_________________________________¶¶_"), nl,
                Translate("__¶¶_________________________________¶¶_"), nl,
                Translate("___¶¶_______________________________¶¶__"), nl,
                Translate("___¶¶______________________________¶¶___"), nl,
                Translate("____¶¶¶__________________________¶¶¶____"), nl,
                Translate("_____¶¶¶¶_¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_¶¶¶¶_____"), nl,
                Translate("_______¶¶¶¶_¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_¶¶¶¶_______"), nl,
                Translate("_________¶¶¶¶_¶¶¶¶¶¶¶¶¶¶¶¶_¶¶¶¶_________"), nl,
                Translate("___________¶¶¶¶¶_¶¶¶¶¶¶¶_¶¶¶¶___________"), nl,
                Translate("______________¶¶¶¶_¶¶¶_¶¶¶______________"), nl,
                Translate("________________¶¶¶_¶_¶¶________________"), nl,
                Translate("_________________¶¶¶_¶¶_________________"), nl,
                Translate("__________________¶¶_¶¶_________________"), nl,
                Translate("__________________¶¶_¶__________________"), nl,
                Translate("__________________¶¶_¶¶_________________"), nl,
                Translate("________________¶¶¶_¶_¶¶¶_______________"), nl,
                Translate("_____________¶¶¶¶¶__¶__¶¶¶¶¶____________"), nl,
                Translate("__________¶¶¶¶¶_____¶_____¶¶¶¶__________"), nl,
                Translate("________¶¶¶¶________¶_______¶¶¶¶¶_______"), nl,
                Translate("_______¶¶¶__________¶__________¶¶¶¶_____"), nl,
                Translate("_____¶¶¶____________¶____________¶¶¶____"), nl,
                Translate("____¶¶¶_____________¶______________¶¶___"), nl,
                Translate("___¶¶¶______________¶_______________¶¶__"), nl,
                Translate("___¶¶_______________¶________________¶¶_"), nl,
                Translate("__¶¶________________¶________________¶¶_"), nl,
                Translate("__¶¶_______________¶¶¶________________¶_"), nl,
                Translate("__¶¶_¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_¶¶"), nl,
                Translate("__¶¶_¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶_¶¶"), nl,
                Translate("__¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶"), nl,
            };
            // - Make it as a String
            string res = string.Concat(dialog);

            // - Dialog text, and the method that will be called in YES option
            //  - the NO option its also avaible but optional.
            ConfigConnector.CreateDialogBoxYesNo(res, YesAction);
        }
    }
}