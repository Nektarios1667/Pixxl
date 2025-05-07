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
            // Name           | RGB color | Variation  | Tags  | Description
            """
            Acid           | 25, 217, 0    | 6  | Liquid               | [Liquid] Burns through weaker materials.
            .Air           | 100, 149, 237 | 0  | Hidden;Gas;Natural   | [Gas] Regular unreactive air that rises when hot.
            Ash            | 54, 54, 54    | 8  | Powder;Natural       | [Powder] Left behind after solids burn.
            BlueFire       | 11, 106, 230  | 13 | Energy;Fire          | [Energy] Superhot fire.
            BlueTorch      | 0, 83, 115    | 3  | Solid;Indestructable | [Solid] Indestructable torch that burns superhot.
            Ceramic        | 115, 44, 14   | 9  | Solid                | [Solid] Good insulator made of clay.
            Chiller        | 33, 30, 130   | 5  | Solid;Indestructable | [Solid] Indestructable material that cools the area.
            Coal           | 13, 13, 13    | 10 | Powder;Fuel;Natural  | [Powder] Solid fuel that burns for a few seconds.
            Concrete       | 160, 160, 160 | 9  | Solid                | [Solid] Stronger material with little destruction from explosives and acid.
            Coolant        | 31, 181, 111  | 8  | Liquid               | [Liquid] Heat-absoribing liquid that turns to gas easily.
            .CoolantVapor  | 56, 224, 146  | 14 | Gas                  | [Gas] Coolant in its vapor form.
            Copper         | 173, 86, 31   | 9  | Solid;Metal;Natural  | [Solid] Metal that transfers heat well.
            Cryofire       | 101, 184, 214 | 20 | Energy;Fire          | [Energy] Supercold fire.
            Diamond        | 24, 149, 171  | 6  | Powder;Natural       | [Powder] Strong, heat-conductive material resistent to almost anything.
            Dirt           | 74, 40, 18    | 15 | Powder;Natural       | [Powder] Earthy dirt.
            Drywall        | 102, 82, 65   | 6  | Solid                | [Solid] Weak plaster used for home interiors.
            Explosive      | 139, 0, 0     | 5  | Powder;Explosive     | [Powder] Small explosive that explodes when it hits something.
            Faucet         | 0, 0, 155     | 3  | Solid;Indestructable | [Solid] Indestructable material that water comes out of.
            Fire           | 189, 46, 21   | 30 | Energy;Fire          | [Energy] A hot flame.
            Flare          | 105, 26, 41   | 5  | Powder               | [Powdered] Flare that burns and releases red smoke for a long time.
            .FlareSmoke    | 171, 34, 60   | 15 | Gas                  | [Gas] Red smoke released by a flare.
            Fuse           | 138, 107, 90  | 10 | Solid;Fuel           | [Solid] Fast burning material used for controlled timing.
            Glass          | 190, 222, 232 | 9  | Solid                | [Solid] Weak glass.
            Glowshard      | 255, 0, 255   | 0  | Powder               | [Powder] Colorful shards stonger than diamond, and prettier too.
            Gold           | 189, 183, 8   | 14 | Powder;Natural;Metal | [Powder] Transfers heat well but not very strong.
            Grass          | 0, 175, 0     | 8  | Powder;Natural       | [Rigid Powder] Grows slowly over time.
            Gravel         | 97, 97, 97    | 18 | Powder;Natural       | [Powder] Crushed of stone.
            Grenade        | 3, 36, 4      | 8  | Powder;Explosive     | [Powder] Creates a weak explosion in a larger area after a short delay.
            Gunpowder      | 46, 46, 46    | 18 | Powder;Explosive     | [Powder] Explodes when in contact with fire.
            Helium         | 168, 213, 227 | 12 | Gas                  | [Gas] Lighter-than-air unreactive gas.
            Ice            | 130, 199, 245 | 18 | Solid;Natural        | [Solid] Frozen water that melts when warmed up.
            Insulation     | 232, 201, 174 | 22 | Solid                | [Solid] Transfers heat at a very slow rate.
            Lava           | 186, 28, 0    | 20 | Natural              | [Liquid] Molten rock.
            Lightning      | 252, 252, 83  | 3  | Energy               | [Energy] 300 million volts of electricity.
            LiquidOxygen   | 186, 189, 219 | 7  | Liquid               | [Liquid] Pure oxygen in liquid state.
            .MeltedPlastic | 171, 232, 132 | 6  | Liquid               | [Liquid] Melted plastic.
            .MeltedRubber  | 30, 30, 30    | 4  | Liquid               | [Liquid] Melted rubber.
            Mercury        | 170, 168, 165 | 4  | Liquid;Metal         | [Liquid] Conductive metal that is liquid at room temp.
            .MoltenCopper  | 222, 108, 35  | 9  | Liquid;Metal         | [Liquid] Liquified copper.
            .MoltenGold    | 219, 213, 15  | 8  | Liquid;Metal         | [Liquid] Liquified gold.
            .MoltenSteel   | 47, 60, 66    | 6  | Liquid;Metal         | [Liquid] Liquified steel.
            Nuke           | 103, 122, 9   | 3  | Powder;Explosive     | [Powder] Creates a massive explosion when it hits the ground.
            Obsidian       | 19, 16, 26    | 6  | Solid                | [Solid] Brittle glass-like rock.
            Oil            | 5, 5, 5       | 4  | Liquid;Natural;Fuel  | [Liquid] Liquid fuel that burns for a time.
            Oxygen         | 213, 215, 237 | 6  | Gas                  | [Gas] Pure oxygen that condenses and solidifes at lower temperatures.
            Plasma         | 187, 57, 227  | 8  | Energy;Gas           | [Energy/Gas] Plasmified air at extreme temperatures.
            Plastic        | 141, 204, 100 | 5  | Solid                | [Solid] Lightweight but durable plastic.
            Potassium      | 61, 66, 62    | 4  | Powder;Metal         | [Powder] Explodes when in contact with water.
            Propane        | 191, 145, 145 | 7  | Gas;Explosive        | [Gas] Explosive lighter-than-air gas.
            Rubber         | 20, 20, 20    | 5  | Solid                | [Solid] Insulator that can melt at higher temperatures.
            Sand           | 184, 144, 24  | 9  | Powder;Natural       | [Powder] Basic sand that turns into glass at high temperatures.
            Smoke          | 120, 100, 100 | 14 | Gas                  | [Gas] Released from fire.
            Snow           | 202, 237, 234 | 21 | Powder               | [Powder] Crystalized ice from the sky.
            Sodium         | 224, 227, 152 | 11 | Powder;Metal         | [Powder] Burns when in contact with water.
            .SolidMercury  | 160, 158, 155 | 5  | Solid;Metal          | [Solid] Solidified conductive metal that is liquid at room temp.
            SolidOxygen    | 157, 161, 201 | 8  | Solid                | [Solid] Pure oxygen in solid state.
            Steam          | 191, 191, 191 | 6  | Gas                  | [Gas] Water in an evaporated state.
            Steel          | 30, 34, 36    | 4  | Solid;Metal          | [Solid] Strengthed steel resistant to small explosions and acid.
            Torch          | 55, 0, 0      | 3  | Solid;Indestructable | [Solid] Indestructable material that burns forever.
            Void           | 45, 6, 48     | 5  | Solid;Indestructable | [Solid] Destroys anything that comes in contact with it.
            Water          | 0, 77, 207    | 9  | Liquid;Natural       | [Liquid] Flowing water.
            Wood           | 43, 17, 2     | 7  | Solid;Natural;Fuel   | [Solid] Solid fuel that burns for a short time.
            """;
    }
}
