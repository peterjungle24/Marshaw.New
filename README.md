Its where i will let my own **Rain World** mod and the code source.
I will improve a lot of things of the last version of my source code, since looks like a pure mess.

# some details here
- ~~Will have a pit~~ that leads to the Mines, likely. i kinda forgot

- A testing layout
- It doesnt say where the things should be, but just a little plan
- > *"These labels tell you the route heads to a location. it doesn't necessarily mean it is literally there"*
- <img width="713" height="328" alt="image" src="https://github.com/user-attachments/assets/f466cf25-bf2e-4fd6-97b0-3e75f9137549" />
> *"We could have had a room where we jumped down from leaf to leaf across the stalks, and later ended up in the mines <br/> But we can see if a open shaft can work.."*
> *"The pit may be a bad idea... It wasn't part of the plan when I made this. I thought all of the shafts were going to be accessed underground"*

# Stalk(er)
```
The arcs at each vertex point are going to be different, and the arc may change over the length of the bend.
You need to cosndier the specific arc between any two points along the bend
Features that extend from the stalk, but do not bend, should not be applied to this calculation. if you can rotate decorating elements, do so
The stalk can bend, but the leaves don't bend with the stalk. The leaves rotate instead
```

<img width="247" height="349" alt="image" src="https://github.com/user-attachments/assets/afb91ed0-5875-49c5-a9ae-0ef1244b1832" />
```md
I feel like you should have some degree of control where the leaves should be, but you can imagine creating part of a plant as a building unit, and have a factory that generates them for you.
Then you have a different one at the top.
You could conveniently hide the sprite overlap by having the next highest mesh draw behind the sprite below it
All of these extra details like at the stem base. You should figure out if it is easier to create those as part of the same base mesh or have extra meshes
This type of model looks really difficult to animate. If you want bending stems, I think you need to have one big stem unit. 
> Warning, slugg, you'll be getting into some high level math if and when you ever decide to get one of these to bend properly.
```
