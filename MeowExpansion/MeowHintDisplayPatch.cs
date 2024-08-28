using Exiled.API.Features;
using Exiled.API.Features.Pools;
using HarmonyLib;
using Hints;
using HintServiceMeow.Core.Utilities;
using PlaceholderAPI.API;
using System.Collections.Generic;
using System.Reflection.Emit;

using static HarmonyLib.AccessTools;

namespace MeowExpansion
{
    [HarmonyPatch(typeof(HintServiceMeow.Core.Utilities.PlayerDisplay), "UpdateHint")]
    public class MeowHintDisplayPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Pool.Get(instructions);

            int index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Stloc_0);

            newInstructions.InsertRange(index + 1, new CodeInstruction[]
            {
                //message
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldloc_0),

                //this
                new CodeInstruction(OpCodes.Ldarg_0),

                //message = Display(string, Referencehub);
                new CodeInstruction(OpCodes.Call, Method(typeof(MeowHintDisplayPatch), nameof(Display))),
                new CodeInstruction(OpCodes.Stloc_0),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Pool.Return(newInstructions);
        }

        public static string Display(string message, PlayerDisplay hub)
        {
            return PlaceholderAPI.API.PlaceholderAPI.SetPlaceholders(Player.Get(hub.ReferenceHub), message);
        }
    }
}
