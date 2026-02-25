using UnityEngine;
using Random = UnityEngine.Random;
using LizardCosmetics;

namespace SourceCode.Creatures.Lizards
{
    sealed class LizoBloingGraphics : LizardGraphics
    {
        // this determines what your lizard's cosmetics will be. the last two are random (ShortBodyScales and TailGeckoScales), the first is guaranteed (TailTuft)
        // also if you need to know what cosmetics there are, open dnspy and go into the LizardCosmetics namespace in Assembly-CSharp
        public LizoBloingGraphics(LizoBloing ow) : base(ow)
        {
            var state = Random.state;
            Random.InitState(ow.abstractPhysicalObject.ID.RandomSeed);
            var spriteIndex = startOfExtraSprites + extraSprites;
            spriteIndex = AddCosmetic(spriteIndex, new TailTuft(this, spriteIndex));
            if (Random.value < .2f)
                spriteIndex = AddCosmetic(spriteIndex, new ShortBodyScales(this, spriteIndex));
            if (Random.value < .3f)
                AddCosmetic(spriteIndex, new TailGeckoScales(this, spriteIndex));
            Random.state = state;
        }
    }
}