using PlaceholderAPI;
using PlaceholderAPI.API.Abstract;
using System.Reflection;

namespace MeowExpansion
{
    public class MeowExpansion : PlaceholderExpansion
    {
        public override string Author { get; set; } = "NotZer0Two";
        public override string Identifier { get; set; } = "meow";
        public override string RequiredPlugin { get; set; } = "HintServiceMeow";

        public override bool CanRegister()
        {
            bool original = base.CanRegister();

            if(original)
                PlaceholderAPIPlugin.HarmonyPatch.PatchAll(typeof(MeowExpansion).Assembly);

            return original;
        }
    }
}
