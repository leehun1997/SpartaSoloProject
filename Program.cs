using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using static ConsoleApp3.Program;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp3
{
    internal class Program
    {
        public enum JobE//언제든지 직업추가 가능
        {
            전사 = 1,
            궁수,
            마법사,
            사제
        }
        public enum MLocation
        {
            방어구,
            무기
        }
        public class Player
        {
            public int Level = 1;//{ get; set; }?
            public string? Name;
            public string? Job;
            public float Atk = 10;
            public int Def = 5;
            public int Health = 100;
            public int Gold = 7500;
            public int exp = 0;
        }
        public struct Item
        {
            public string name;
            public string effect;
            public int str;
            public string explain;
            public int price;
            public bool buy = false;
            public bool equip = false;
            public MLocation location; //0 방어구 1 무기

            public Item(string _name, string _effect, int _str, string _explain, int _price, MLocation _location) //구조체 생성자
            {
                name = _name;
                effect = _effect;
                str = _str;
                explain = _explain;
                price = _price;
                location = _location;
            }
            public void StoreInterface() //구조체의 메서드
            {
                if (!buy)
                    Console.WriteLine($"- {name} | {effect} {str} | {explain} | {price} G");
                else
                    Console.WriteLine($"- {name} | {effect} {str} | {explain} | 구매완료");
            }
            public void InventoryInterface() //구조체의 메서드
            {
                if (equip)
                    Console.WriteLine($"[E][{location}] {name} | {effect} {str}");
                else
                    Console.WriteLine($"[ ][{location}] {name} | {effect} {str}");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("스파르타마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서는 던전으로 들어가기전 활동을 할 수 있습니다.\n");

            Console.WriteLine("당신의 이름을 알려주세요.");
            string pname = Console.ReadLine();
            Console.WriteLine();

            Player player = new Player();//플레이어 정보 생성
            player.Name = pname;
            player.Job = JobSelect();

            //아이템도 이후에 추가가 용이하게 만듬
            Item item1 = new Item("수련자 갑옷", "DEF", 5, "수련에 도움을 주는 갑옷입니다.", 1000, MLocation.방어구);
            Item item2 = new Item("무쇠갑옷", "DEF", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000, MLocation.방어구);
            Item item3 = new Item("스파르타의 갑옷", "DEF", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, MLocation.방어구);
            Item item4 = new Item("낡은 검", "ATK", 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600, MLocation.무기);
            Item item5 = new Item("청동 도끼", "ATK", 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500, MLocation.무기);
            Item item6 = new Item("스파르타의 창", "ATK", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3000, MLocation.무기);

            List<Item> items = new List<Item>();
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);
            items.Add(item4);
            items.Add(item5);
            items.Add(item6);

            GameMenu(player, items); // 게임시작

            //Func<int, int, int> addFuc = Add;
            //int result = addFuc(3, 5);
            //Console.WriteLine(result);

            //Action<String> prM = printMessage;
            //prM("Hello!");

            //Action<String> prM2 = (message) => { Console.WriteLine(message); };
            //prM2("Hello2!");
        }

        static string JobSelect()
        {
            foreach (JobE job in Enum.GetValues(typeof(JobE)))
            {
                Console.WriteLine($"{job} : {(int)job}");
            }
            Console.WriteLine("당신의 직업을 선택해주세요.\n");
            string pjob = Console.ReadLine();
            Console.WriteLine();

            foreach (JobE job in Enum.GetValues(typeof(JobE)))
            {
                int pjI; //player job int
                int.TryParse(pjob, out pjI);
                if (pjob == job.ToString() || pjI == (int)job)//번호나 이름으로 직업 넣어주기 ***tryparse에 실패 시 false 값과 int 값 0을 반환한다. 그래서 직업의 시작을 0이 아닌 1로 변경 
                    return job.ToString();
                else if (pjob == "Hidden")
                    return "Hidden";
            }
            return "초보모험가";
        }

        static void GameMenu(Player player, List<Item> items)
        {
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기\n");

            int select = GetBehaviorNum();

            switch (select)
            {
                case 1: ShowStatus(player, items); break;
                case 2: ShowInventory(player, items); break;
                case 3: ShowStore(player, items); break;
                case 4: ShowDungun(player, items); break;
                case 5: ShowRest(player, items); break;
                default: Console.WriteLine("* 범위 내의 숫자를 입력해 주세요."); GameMenu(player, items); break;
            }
        }

        static int GetBehaviorNum()
        {
            Console.WriteLine("원하시는 행동을 입력해 주세요.");
            string s = Console.ReadLine();

            int selectNum;
            if (int.TryParse(s, out selectNum))
                Console.WriteLine();
            else
            {
                Console.WriteLine("* 숫자를 입력해 주세요.");
                selectNum = GetBehaviorNum();
            }

            return selectNum;
        }

        static void ShowStatus(Player player, List<Item> items)
        {
            Console.WriteLine($"Lv. {player.Level}");
            Console.WriteLine($"{player.Name} ({player.Job})");
            Console.WriteLine($"Atk : {player.Atk}");
            Console.WriteLine($"Def : {player.Def}");
            Console.WriteLine($"HP : {player.Health}");
            Console.WriteLine($"{player.Gold} G\n");
            Console.WriteLine("0. 나가기\n");

            int select = GetBehaviorNum();

            switch (select)
            {
                case 0: GameMenu(player, items); break;
                default: Console.WriteLine("* 범위 내의 숫자를 입력해 주세요."); ShowStatus(player, items); break;
            }
        }
        static void ShowInventory(Player player, List<Item> items)
        {
            Console.WriteLine("---인벤토리---");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");
            foreach (Item item in items)
            {
                if (item.buy)
                    item.InventoryInterface();
            }
            Console.WriteLine();

            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기\n");

            int select = GetBehaviorNum();

            switch (select)
            {
                case 0: GameMenu(player, items); break;
                case 1: EquipMenu(player, items); break;
                default: Console.WriteLine("* 범위 내의 숫자를 입력해 주세요."); ShowInventory(player, items); break;
            }
        }
        static void EquipMenu(Player player, List<Item> items)
        {
            int count = 1;
            Console.WriteLine("---인벤토리_장착관리---");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");
            foreach (Item item in items)
            {
                if(item.buy)
                {
                    Console.Write($"{count}. ");
                    item.InventoryInterface();
                    count++;
                }
            }
            Console.WriteLine();

            Console.WriteLine("0. 나가기\n");

            int select = GetBehaviorNum();

            if (select == 0)
                ShowInventory(player, items);

            else if (count <= select)
            {
                Console.WriteLine("* 범위 내의 숫자를 입력해 주세요.");
                EquipMenu(player, items);
            }
            else
            {
                count = 1;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].buy == true)
                    {
                        if (count == select)
                        {  
                            Item titem = items[i];
                            items[i] = ItemEquip(player, items, titem);
                            break;
                        }
                        count++;
                    }
                }
                EquipMenu(player, items);
            }

        }
        static Item ItemEquip(Player player, List<Item> items, Item item)
        {
            if (item.equip)
            {
                Console.WriteLine($"{item.name} 장착 해제!");

                if (item.effect == "ATK")
                {
                    player.Atk -= item.str;
                }
                else if (item.effect == "DEF")
                {
                    player.Def -= item.str;
                }
            }
            else
            {
                ItemLocationCheck(player, items, item);
                Console.WriteLine($"{item.name} 장착 완료!");

                if (item.effect == "ATK")
                {
                    player.Atk += item.str;
                }
                else if (item.effect == "DEF")
                {
                    player.Def += item.str;
                }
            }

            item.equip = !item.equip;

            return item;
        }
        static void ItemLocationCheck(Player player, List<Item> items, Item item)
        {
            for(int i=0;i<items.Count; i++)
            {
                if (items[i].equip && items[i].location == item.location && items[i].name != item.name)
                {
                    Item t = items[i];
                    t = ItemEquip(player, items, t);
                    items[i] = t;
                    break;
                }
            }
        }
        static void ShowStore(Player player, List<Item> items)
        {
            Console.WriteLine("---상점---");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G\n");

            Console.WriteLine("[아이템 목록]");
            foreach (Item item in items)
            {
                item.StoreInterface();
            }
            Console.WriteLine();

            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기\n");

            int select = GetBehaviorNum();

            switch (select)
            {
                case 0: GameMenu(player, items); break;
                case 1: BuyMenu(player, items); break;
                case 2: SellMenu(player, items); break;
                default: Console.WriteLine("* 범위 내의 숫자를 입력해 주세요."); ShowStore(player, items); break;
            }
        }
        static void BuyMenu(Player player, List<Item> items)
        {
            int count = 1;
            Console.WriteLine("---상점_아이템 구매---");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G\n");

            foreach (Item item in items)
            {
                Console.Write($"{count}. ");
                item.StoreInterface();
                count++;
            }

            Console.WriteLine("0. 나가기\n");

            int select = GetBehaviorNum();

            if (select == 0)
                ShowStore(player, items);

            else if (count <= select)
            {
                Console.WriteLine("* 범위 내의 숫자를 입력해 주세요.");
                BuyMenu(player, items);
            }
            else
            {
                Item titem = items[select - 1];
                if (titem.buy)
                {
                    Console.WriteLine("이미 구입한 아이템입니다.");
                    BuyMenu(player, items);
                }
                else if (titem.price > player.Gold)
                {
                    Console.WriteLine("골드가 부족합니다.");
                    BuyMenu(player, items);
                }
                else
                {
                    Console.WriteLine("구매완료.");
                    player.Gold -= titem.price;
                    titem.buy = true;
                    items[select - 1] = titem;
                    BuyMenu(player, items);
                }
            }
        }
        static void SellMenu(Player player, List<Item> items)
        {
            int count = 1;

            Console.WriteLine("---상점_아이템 판매---");
            Console.WriteLine("보유한 아이템을 상점에 판매할 수 있습니다.\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G\n");

            foreach (Item item in items)
            {
                if (item.buy)
                {
                    Item temitem = item;
                    temitem.buy = false;
                    Console.Write($"{count}. ");
                    temitem.StoreInterface();
                    count++;
                }
            }
            Console.WriteLine();

            Console.WriteLine("0. 나가기\n");

            int select = GetBehaviorNum();

            if (select == 0)
                ShowStore(player, items);

            else if (count <= select)
            {
                Console.WriteLine("* 범위 내의 숫자를 입력해 주세요.");
                SellMenu(player, items);
            }
            else
            {
                count = 1;
                for(int i =0;i<items.Count ;i++)
                {
                    if (items[i].buy == true)
                    {
                        if (count == select)
                        {
                            Item titem = items[i];
                            if (items[i].equip)//만약 아이템이 장착중이라면 해제
                                titem = ItemEquip(player,items, titem);
                            items[i] = ItemSell(player, titem);
                            break;
                        }
                        count++;
                    }
                }
                SellMenu(player, items);
            }
        }
        static Item ItemSell(Player player, Item item)
        {
            Console.WriteLine("판매 완료!");
            player.Gold += (int)(item.price * 0.85f);
            item.buy = false;

            return item;
        }
        static void ShowDungun(Player player, List<Item> items)
        {
            Console.WriteLine("---던전 입장---");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine($"[현재 공격력 : {player.Atk}] [현재 방어력 : {player.Def}]\n");

            Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장");
            Console.WriteLine("0. 나가기\n");

            int select = GetBehaviorNum();

            switch (select)
            {
                case 0: GameMenu(player, items); break;
                case 1: new Dungun(1000,5,0,"쉬운 던전").Clear(player, items); break;
                case 2: new Dungun(1700, 11, 1, "일반 던전").Clear(player, items); break;
                case 3: new Dungun(2500, 17, 2, "어려운 던전").Clear(player, items); break;
                default: Console.WriteLine("* 범위 내의 숫자를 입력해 주세요."); ShowDungun(player, items); break;
            }

            ShowDungun(player,items);
        }
        public class Dungun
        {
            int minDamage = 20;
            int maxDamage = 35;
            int damage = 0;
            float bonus = 0;

            public int clearGold = 0;
            public int DEFCutLine = 0;
            public int diff = 0;
            public string name;
            public Dungun(int _clearGold, int _DEFCutLine, int _diff, string _name)
            {
                clearGold = _clearGold;
                DEFCutLine = _DEFCutLine;
                diff = _diff;
                name = _name;
            }

            public void Clear(Player player, List<Item> items)
            {
                if (DEFCutLine > player.Def)
                {
                    int rand = new Random().Next(10);
                    if (rand < 4+diff)
                    {
                        Console.WriteLine("던전 클리어에 실패했습니다.");
                        Console.WriteLine("방어력을 더 높이는 것을 추천합니다.\n");

                        Console.WriteLine("[탐험 결과]");
                        Console.WriteLine($"Hp :{player.Health} -> {(int)(player.Health/2)}\nGold : {player.Gold}\n");
                        player.Health = (int)(player.Health / 2);
                        return;
                    }
                }                
                damage = player.Def - DEFCutLine;
                bonus = player.Atk;

                damage = new Random().Next(minDamage - damage, maxDamage - damage);
                bonus = new Random().Next((int)bonus, (int)bonus * 2);                

                Console.WriteLine("축하합니다!");
                Console.WriteLine($"{name}을 클리어 하였습니다!\n");

                Console.WriteLine("[탐험 결과]");
                Console.WriteLine($"Hp :{player.Health} -> {player.Health - damage}");
                Console.WriteLine($"Gold: { player.Gold} -> { player.Gold + (int)(clearGold * (100 + bonus) / 100)}");

                if(player.Level == player.exp + 1)
                {
                    Console.WriteLine("레벨 업!");
                    Console.WriteLine($"Lv : {player.Level} -> {player.Level + 1}\n");
                    player.Level += 1;
                    player.Atk += 0.5f;
                    player.Def += 1;
                    player.exp = 0;
                }
                else
                {
                    Console.WriteLine("경험치를 회득했습니다.");
                    Console.WriteLine($"Exp : {player.exp}/{player.Level} -> {player.exp + 1}/{player.Level}\n");
                    player.exp += 1;
                }

                player.Health -= damage;
                player.Gold += (int)(clearGold * (100 + bonus) / 100);
            }            
        }

        static void ShowRest(Player player, List<Item> items)
        {
            Console.WriteLine("---휴식하기---");
            Console.WriteLine($"500 G 를 내면 체력을 회복 할 수 있습니다. (보유 골드 : {player.Gold} G)\n");

            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기.\n");

            int select = GetBehaviorNum();

            switch (select)
            {
                case 0: GameMenu(player, items); break;
                case 1: Rest(player); ShowRest(player, items); break;
                default: Console.WriteLine("* 범위 내의 숫자를 입력해 주세요."); GameMenu(player, items); break;
            }
        }
        static void Rest(Player player)
        {
            if (player.Gold < 500)
                Console.WriteLine("Gold가 부족합니다!");
            else
            {
                Console.WriteLine("휴식을 완료했습니다.");
                player.Gold -= 500;
                player.Health = 100;
            }
        }
        //class EasyDungun : Dungun
        //{
        //    public new int clearGold = 1000;
        //    public new int DEFCutLine = 5;
        //    public new int diff = 4;

        //    public int damage = 0;
        //    public int bonus = 0;
        //    public override void Clear(Player player)
        //    {
        //        if (DEFCutLine > player.Def)
        //        {
        //            int rand = new Random().Next(10);
        //            if (rand < diff)
        //            {
        //                Console.WriteLine("던전 실패!");
        //                player.Health = (int)(player.Health / 2);
        //                return;
        //            }
        //        }
        //        Console.WriteLine($"{this.diff} 던전 클리어 : {this.clearGold} G, -{this.damage} HP");
        //        damage = player.Def - DEFCutLine;
        //        bonus = player.Atk;
        //        damage = new Random().Next(minDamage - damage, maxDamage - damage);
        //        bonus = new Random().Next(bonus, bonus * 2);
        //        player.Health -= damage;
        //        player.Gold += (int)(clearGold * (100 + bonus) / 100);
        //    }
        //}
        //class NormalDungun : Dungun
        //{
        //    public new int clearGold = 1700;
        //    public new int DEFCutLine = 11;
        //    public new int diff = 5;

        //    public new int damage = 0;
        //    public new int bonus = 0;
        //    public override void Clear(Player player)
        //    {
        //        if (DEFCutLine > player.Def)
        //        {
        //            int rand = new Random().Next(10);
        //            if (rand < diff)
        //            {
        //                Console.WriteLine("던전 실패!");
        //                player.Health = (int)(player.Health / 2);
        //                return;
        //            }
        //        }
        //        Console.WriteLine($"{this.diff} 던전 클리어 : {this.clearGold} G, -{this.damage} HP");
        //        damage = player.Def - DEFCutLine;
        //        bonus = player.Atk;
        //        damage = new Random().Next(minDamage - damage, maxDamage - damage);
        //        bonus = new Random().Next(bonus, bonus * 2);
        //        player.Health -= damage;
        //        player.Gold += (int)(clearGold * (100 + bonus) / 100);
        //    }
        //}
        //class HardDungun : Dungun
        //{
        //    public new int clearGold = 2500;
        //    public new int DEFCutLine = 17;
        //    public new int diff = 6;

        //    public new int damage = 0;
        //    public new int bonus = 0;
        //    public override void Clear(Player player)
        //    {
        //        if (DEFCutLine > player.Def)
        //        {
        //            int rand = new Random().Next(10);
        //            if (rand < diff)
        //            {
        //                Console.WriteLine("던전 실패!");
        //                player.Health = (int)(player.Health / 2);
        //                return;
        //            }
        //        }
        //        Console.WriteLine($"{this.diff} 던전 클리어 : {this.clearGold} G, -{this.damage} HP");
        //        damage = player.Def - DEFCutLine;
        //        bonus = player.Atk;
        //        damage = new Random().Next(minDamage - damage, maxDamage - damage);
        //        bonus = new Random().Next(bonus, bonus * 2);
        //        player.Health -= damage;
        //        player.Gold += (int)(clearGold * (100 + bonus) / 100);
        //    }
        //}
    }
}