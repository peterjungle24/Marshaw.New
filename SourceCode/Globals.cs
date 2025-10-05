// i learned!
// :monksilly:

// System | Unity
global using System;
global using System.Collections.Generic;
global using System.Reflection;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using System.Runtime.CompilerServices;

// BepInEx
global using BepInEx;
global using BepInEx.Logging;

// Unity | Mono
global using UnityEngine;
global using MonoMod.Cil;
global using static UnityEngine.Random;

// Slugbase (nescesary for that json file)
global using SlugBase;

// Rain World ones
global using MoreSlugcats;
global using DevInterface;
global using Menu.Remix.MixedUI;
global using RWCustom;
global using objPhy = AbstractPhysicalObject;
global using objType = AbstractPhysicalObject.AbstractObjectType;
global using DLC_ObjType = DLCSharedEnums.AbstractObjectType;

// Mods Libraries
global using static Pom.Pom;
//global using LogUtils;
//global using LogUtils.Diagnostics;
//global using LogUtils.Diagnostics.Tools;
//global using LogUtils.Diagnostics.Tests;
//global using LogUtils.Enums;
//global using LogUtils.Helpers;

// Shortened Names
global using Randomf = UnityEngine.Random;
global using Debugf = UnityEngine.Debug;
global using LogConsole = LogUtils.Console;

// My Namespaces