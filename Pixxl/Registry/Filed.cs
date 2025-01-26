using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixxl.Registry
{
    public static class Filed
    {
        public static string Materials =
            // Name           | RGB color | Variation  | Description
            """
            Acid           | 25, 217, 0    | 6  | [Liquid] Burns through weaker materials.
            .Air           | 100, 149, 237 | 0  | [Gas]
            Ash            | 54, 54, 54    | 8  | [Powder] Left behind after solids burn.
            BlueFire       | 11, 106, 230  | 13 | [Energy] Superhot fire.
            BlueTorch      | 0, 83, 115    | 3  | [Solid] Indestructable torch that burns superhot.
            Chiller        | 33, 30, 130   | 5  | [Chiller] Indestructable material that cools the area.
            Coal           | 13, 13, 13    | 10 | [Powder] Solid fuel that burns for a few seconds.
            Concrete       | 160, 160, 160 | 9  | [Solid] Stronger material with little destruction from explosives and acid.
            Coolant        | 31, 181, 111  | 8  | [Liquid] Heat-absoribing liquid that turns to gas easily.
            .CoolantVapor  | 56, 224, 146  | 14 | [Gas]
            Copper         | 173, 86, 31   | 9  | [Solid] Metal that transfers heat well.
            Diamond        | 24, 149, 171  | 6  | [Powder] Strong, heat-conductive material resistent to almost anything.
            Dirt           | 74, 40, 18    | 15 | [Powder] Earthy dirt.
            Explosive      | 139, 0, 0     | 5  | [Powder] Small explosive that explodes when it hits something.
            Faucet         | 0, 0, 155     | 3  | [Solid] Indestructable material that water comes out of.
            Fire           | 189, 46, 21   | 30 | [Energy] A hot flame.
            Flare          | 105, 26, 41   | 5  | [Powdered] Flare that burns and releases red smoke for a long time.
            .FlareSmoke    | 171, 34, 60   | 15 | [Gas]
            Fuse           | 138, 107, 90  | 10 | [Solid] Fast burning material used for controlled timing.
            Glass          | 190, 222, 232 | 9  | [Solid] Weak glass.
            Glowshard      | 255, 0, 255   | 0  | [Powder] Colorful shards stonger than diamond, and prettier too.
            Gold           | 189, 183, 8   | 14 | [Powder] Transfers heat well but not very strong.
            Grass          | 0, 175, 0     | 8  | [Rigid Powder] Grows slowly over time.
            Gravel         | 97, 97, 97    | 18 | [Powder] Crushed of stone.
            Grenade        | 3, 36, 4      | 8  | [Powder] Creates a weak explosion in a larger area after a short delay.
            Helium         | 168, 213, 227 | 12 | [Gas] Lighter-than-air unreactive gas.
            Ice            | 130, 199, 245 | 18 | [Solid] Frozen water that melts when warmed up.
            Insulation     | 245, 245, 245 | 18 | [Solid] Transfers heat at a very slow rate.
            Lava           | 186, 28, 0    | 20 | [Liquid] Liquified molten rock.
            .MoltenCopper  | 222, 108, 35  | 9  | [Liquid]
            .MoltenGold    | 219, 213, 15  | 8  | [Liquid]
            .MoltenSteel   | 47, 60, 66    | 6  | [Liquid]
            Nuke           | 103, 122, 9   | 3  | [Solid] Creates a massive explosion when it hits the ground.
            Oil            | 5, 5, 5       | 4  | [Liquid] Liquid fuel that burns for a time.
            Plasma         | 187, 57, 227  | 8  | [Energy/Gas] Plasmified air at extreme temperatures.
            Potassium      | 61, 66, 62    | 4  | [Powder] Explodes when in contact with water.
            Propane        | 191, 145, 145 | 7  | [Gas] Explosive lighter-than-air gas.
            Sand           | 184, 144, 24  | 9  | [Powder] Basic sand that turns into glass at high temperatures.
            Smoke          | 120, 100, 100 | 14 | [Gas] Released from fire.
            Sodium         | 224, 227, 152 | 11 | [Powder] Burns when in contact with water.
            Steam          | 191, 191, 191 | 6  | [Gas] Water in a gaseous state.
            Steel          | 30, 34, 36    | 4  | [Solid] Strengthed steel resistant to small explosions and acid.
            Torch          | 55, 0, 0      | 3  | [Solid] Indestructable material that burns forever.
            Void           | 45, 6, 48     | 5  | [Solid] Destroys anything that comes in contact with it.
            Water          | 0, 77, 207    | 9  | [Liquid] Flowing water.
            Wood           | 43, 17, 2     | 7  | [Solid] Solid fuel that burns for a short time.
            """;
    }
}
