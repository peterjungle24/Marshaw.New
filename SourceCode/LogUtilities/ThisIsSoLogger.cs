using LogUtils;
using LogUtils.Diagnostics;
using LogUtils.Diagnostics.Tools;
using LogUtils.Diagnostics.Tests;
using LogUtils.Enums;
using LogUtils.Helpers;
using System.Reflection;

namespace SourceCode.LogUtilities
{
    public class ThisIsSoLogger : TestSuite, ITestable
    {
        public string Name => "<This Is So Logger Unit Test>";
        private LogUtils.Logger log;
        private TestCase testCase;
        private Func<Color, string> f;
        private bool data;
        private string report;
        
        public ThisIsSoLogger()
        {
            // instances a TestCase with my Name field
            testCase = new TestCase(Name);
            // instances a new Logger with Plugin.Logger as target
            log = new LogUtils.Logger(Plugin.logger);
            // just assigns the "f" to this method (foreground color)
            f = LogConsole.AnsiColorConverter.AnsiToForeground;

            // add tests on this current assembly.
            // not that sure what is assembly but ok
            base.AddTests(Assembly.GetExecutingAssembly());
        }

        public void Test()
        {
            // assert thing
            testCase.AssertThat<bool>(data).IsTrue();
        }
        [PostTest]public void Results()
        {
            // create reports
            report = testCase.CreateReport();
            // log reports
            log.LogImportant($"{f(new Color(66, 199, 101) )}{report}");
        }
    }
}

// This works, but is not the proper way to use the system
/* Creating a log instance

for this, i could create a "LogUtils.Logger" instance first
--------------------
private LogUtils.Logger log;
--------------------

and then, i need to asign it
but since it REQUIRES a "ILogger" and "ManualLogSource" its a ILogger, then its easy
--------------------
// somewhere
log = new LogUtils.Logger(Plugin.logger);
--------------------

and done!

*/
/* Color to logs..

currently on this version of LogUtils i am using, theres a way to add color to console (consoly only, duh)
this way its just for color a foreground, but can be also used to background
for this, i make a field of "Func<Color, string>" to a specific method
-------------------
private Func<Color, string> f;
-------------------

and then i assign it, likely on "ctor" is better
-------------------
public MyUnitToTest()
{
    f = LogConsole.AnsiColorConverter.AnsiToForeground;
}
-------------------

and.. done!
if you want to use it, its like this
-------------------
log.Log($"{f(<any color here>) }My Text!");
// or
log.Log(string.Format("{0}My Text!", <color here>) );
-------------------

*/
/* Basic Unit Test (test methods)

So, we start creating a class that inherits both "TestSuite" and "ITestable"
("ITestable" requires a "Name" field and "Test" method)
--------------------
public class MyUnitToTest : TestSuite, ITestable
{
    public string Name => <value here>;
    
    public void Test()
    {
    }
}
--------------------

on the "Test" method, we can add our testing.. literally
--------------------
public void Test()
{
    ... // our test here
}
--------------------

and done.
this is our basic Test class so far

-----------------------------------------------

public class MyUnitToTest : TestSuite, ITestable
{
    public string Name => <value here>;
    private TestCase testCase;
    private string report;
        
    public MyUnitToTest()
    {
        testCase = new TestCase(Name);
    }

    public void Test()
    {
        ... // test things
    }
}
-----------------------------------------------

*/
/* Basic Unit Test (results)

now we are on a Results part

create a another method called "Results"
NOTE: It needs to have a "PostTest" attribute
--------------------
[PostTest]
public void Results()
{
    
}
--------------------

for the results appear on console, we first needs to do a little more..

create a string field, that will store our REPORTS
looks like it needs to create reports to log them
but sinde it returns String, then i create a field
--------------------
private string report;  // any name but since it will return our report, i will call it report.
--------------------

next, we should create reports, on "Results" method using our "testCase" field
--------------------
[PostTest]
public void Results()
{
    report = testCase.CreateReport();
}
--------------------

now, in order to make it work, we should get our Assembly and add the assembly to tests
NOTE: DONT ADD IT TO "Results" OR "Test" METHOD, TRY TO MOVE TO YOUR ctor
--------------------
using System.Reflection;

[PostTest]
public MyUnitToTest()
{
    ...
    base.AddTests(Assembly.GetExecutingAssembly() );
}
--------------------

and done!
not sure if it will log, so, for make sure that it will log, i get a LogUtils.Logger instance and log them
-------------------
[PostTest]
public void Results()
{
    report = testCase.CreateReport();
    
    log.LogImportant($"{report}");
}
-------------------

so, our class is like this now.

-----------------------------------------------
public class MyUnitToTest : TestSuite, ITestable
{
    public string Name => <value here>;
    private TestCase testCase;
    private string report;
        
    public MyUnitToTest()
    {
        testCase = new TestCase(Name);
        base.AddTests(Assembly.GetExecutingAssembly() );
    }

    public void Test()
    {
        ... // test things
    }
    
    [PostTest]
    public void Results()
    {
        report = testCase.CreateReport();
    
        log.LogImportant($"{report}");
    }
}
-----------------------------------------------

*/
/* Basic Unit Test (using)

now the rest will be on our Plugin class
lets create a field with our class type
----------------------
private static MyUnitToTest unitTest;
----------------------

now, since we are on RW Modding context, then a recommended place its "RainWorld.OnModsInit"
----------------------
private void OnModsInit(...)
{
    unitTest = new MyUnitToTest();
}
----------------------

now in any hook that you plan to run a test, you can just make this
----------------------
private void SomeHook(orig, ...)
{
    unitTest.Test();
    unitTest.Results();

    orig(...);
}
----------------------

and done!

*/