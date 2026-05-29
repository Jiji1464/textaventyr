using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace textaventyr
{
    internal class Program
    {
        public static int bossRoomNumber = 20;
        public static int roomNumber = 1;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to \"THE WIZARD'S TOWER\", the best game EVER made!");
            Console.WriteLine("Note: The game has some bugs in its current state. If you see lines repeating, do not worry. It still works.");
            Console.WriteLine("In its current state, the game has 20 rooms.");
            Player player1 = null;
            while (player1 == null)
            {
                Console.WriteLine($"In order to begin playing, please select your class:\n[1] Warrior (Has lots of HP, but less mana)\n[2] Knight (THE all rounder)\n[3] Mage (has lots of mana, but less HP)");
                int.TryParse(Console.ReadLine(), out int classChoice);
                player1 = ChooseClass(classChoice);
            }
            Console.WriteLine("Now, enter your name!");
            player1.Name = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("You wake up in a strange room. Why and how did you get there? You can't seem to remember.");
            Thread.Sleep(2000);
            Console.WriteLine("After a while, you come to your senses. \"Oh, right!\", you think to yourself. You were going to try and kill whatever corrupt god had ruined the country by cursing people at random.");
            Thread.Sleep(5000);
            Console.WriteLine("You get up and ready yourself to venture up into the tower and slay the god, no matter what it takes.");
            Thread.Sleep(5000);
            Console.Clear();
            Console.WriteLine("Faking load sequence for immersion");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write($"   Done");
            Thread.Sleep(1000);
            Console.Clear();
            player1.ShowStats();
            Room currentRoom = new BasicRoom();
            while (player1.Health > 0)
            {
                Console.Clear();
                currentRoom = currentRoom.Enter(player1);
                if (Program.roomNumber > Program.bossRoomNumber)
                {
                    Console.Clear();
                    Console.WriteLine("You exit the room only to be met by fresh air and a view of the outside world");
                    Thread.Sleep(2000);
                    Console.WriteLine("Seems like you're on a balcony. You look back inside the tower, where the fallen god has turnt to ash.");
                    Thread.Sleep(2000);
                    Console.WriteLine("");
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.WriteLine("Seems like there's a portal where the body fell");
                    Console.WriteLine("Seeing the opportunity for another adventure, you take a step forward.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("\"Perhaps there are more corrupt gods to slay somwehere in there\", you think.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("");
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Clear();
                    Console.WriteLine("You step forward into the light");
                    Thread.Sleep(1000);
                    Console.Clear();
                    Console.WriteLine("================================================");
                    Console.WriteLine("");
                    Console.WriteLine("                     YOU WIN!");
                    Console.WriteLine("");
                    Console.WriteLine("================================================");
                    Console.WriteLine($"Credits: \n Programming: Yngve Schulthes \n Idea: Yngve Schulthes \n Everything else: Yngve Schulthes\n\n Special thanks:\n  You (for playing the game <3)");
                    Console.WriteLine("Press any key to close the program");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
        }
        public static Player ChooseClass(int choice)
        {
            switch (choice)
            {
                case 1:
                    Console.WriteLine("You have chosen to be a Warrior.");
                    return new Warrior();
                case 2:
                    Console.WriteLine("You have chosen to be a Knight.");
                    return new Knight();
                case 3:
                    Console.WriteLine("You have chosen to be a Mage.");
                    return new Mage();
                default:
                    Console.WriteLine("You either fat fingered a button or.... uhhhhhhh i don't even know [Invalid input]");
                    return null;
            }
        }
    }
    interface IHealthManager
    {
        void ResetHealth();
        void FullHeal();
        void GetAttacked(int amount);
    }
    class Player : IHealthManager
    {
        public Player()
        {
            maxHealth = 10;
            health = maxHealth;
            maxMana = 10;
            mana = 5;
            className = "Basic";
            castSpellMultiplier = 2;
            meeleeAttackMultiplier = 2;
        }
        public List<string> Inventory { get; set; } = new List<string>();
        private string name = "Unknown";
        public string? Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = "Unknown";
                }
                name = value;
            }
        }
        public int maxHealth;
        private int health;
        public int maxMana;
        private int mana;
        public int potentialManaGainedUponRoundStart = 5;
        public string className;
        public int castSpellMultiplier;
        public int meeleeAttackMultiplier;
        bool hasChosenToDodge = false;
        bool hasDodgedLastTurn = false;
        bool isDodgingThisTurn = false;
        public int dodgeChance = 35;
        private Random randomizer = new Random();
        public int Health
        {
            get { return health; }
            set
            {
                if (value < 0)
                { value = 0; }
                else if (value > maxHealth)
                { value = maxHealth; }
                health = value;
            }
        }
        public int Mana
        {
            get { return mana; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > maxMana) { value = maxMana; }
                mana = value;
            }
        }
        public void ResetHealth()
        {
            maxHealth = 10;
            Health = maxHealth;
        }
        public void FullHeal()
        {
            Health = maxHealth;
        }
        public void ShowStats()
        {
            Console.WriteLine($"======================\nPLAYER STATE\n\nCLASS: {className}\nNAME: {name}\nHEALTH: {Health}/{maxHealth}\nMANA: {Mana}/{maxMana}\n======================");
        }
        public void GetAttacked(int amount)
        {
            int potentialDamageTaken = amount;
            if (isDodgingThisTurn == true)
            {
                potentialDamageTaken = 0;
                hasDodgedLastTurn = true;
            }
            else
            {
                hasDodgedLastTurn = false;
            }
            TakeDamage(potentialDamageTaken);
        }
        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (amount == 0)
            {
                Console.WriteLine($"You successfully dodged the incoming attack!\n");
            }
            else
            {
                Console.WriteLine($"You took damage!\nDamage taken: {amount}!\n");
            }
        }
        public void Heal(int amount)
        {
            Health += amount;
            Console.WriteLine($"You've healed {amount} points of health!\n");
        }
        public void LoseMana(int amount)
        {
            Mana -= amount;
            Console.WriteLine($"You lost {amount} mana points!\n");
            if (Mana == 0)
            {
                Console.WriteLine($"Your mana is depleted!\n");
            }
        }
        public void GainMana(int amount)
        {
            Mana += amount;
            Console.WriteLine($"You've gained {amount} mana points!\n");
        }
        //dodging
        public void UpdateDodge()
        {
            if (hasDodgedLastTurn)
            { ResetDodge(); }
            else
            { dodgeChance += 15; }
        }
        public void ResetDodge()
        {
            dodgeChance = 35;
        }
        public void DoDodgeCalculation()
        {
            int dodgingRollResult = randomizer.Next(1, (101));
            if (dodgingRollResult <= dodgeChance)
            {
                isDodgingThisTurn = true;
            }
            else
            {
                isDodgingThisTurn = false;
            }
        }
        public void TryDodge()
        {
            UpdateDodge();
            DoDodgeCalculation();
        }
        public void StartTurn(Enemy enemy) // Note to self: I have to pass in the name of an enemy.
        {
            isDodgingThisTurn = false;
            GainMana(randomizer.Next((potentialManaGainedUponRoundStart - 4), potentialManaGainedUponRoundStart));
            ShowStats();

            Console.WriteLine("");
            bool validChoiceMade = false;
            while (!validChoiceMade)
            {
                Console.WriteLine($"It is your turn to act. What will you do?\nPress [X] to fight,\nPress [C] to attempt to dodge.\nPress [V] to look through your inventory and potentially use items (if you do not use an item, your turn will be skipped).\nPress [B] to focus and gain {potentialManaGainedUponRoundStart * 1} mana.");
                string? choiceInput = (Console.ReadLine()?.ToLower());
                validChoiceMade = makeGameplayChoice(choiceInput, enemy);
                Console.WriteLine("");
            }
            Console.Clear();
        }
        public bool makeGameplayChoice(string choice, Enemy enemy)
        {
            switch (choice)
            {
                case "x":
                    bool validAttackChoiceMade = false;
                    Console.Clear();
                    Console.WriteLine("You chose to attack.");
                    while (!validAttackChoiceMade)
                    {
                        Console.WriteLine($"Choose your attack.\nEnter [1] to attempt to hit the enemy\nEnter [2] to slash the opponent using a sharp object. [Requires throwing knife in inventory] {((!Inventory.Contains("Throwing knife")) ? "[NOT AVAILABLE]" : "")} \nEnter [3] to cast fireball. [MANA COST: 7]{(Mana < 7 ? " [NOT ENOUGH MANA]" : "")} \nEnter [4] to cast explosion. [MANA COST: 14]{(Mana < 14 ? " [NOT ENOUGH MANA]" : "")}");
                        int.TryParse(Console.ReadLine(), out int choiceInput);
                        validAttackChoiceMade = makeAttackChoice(choiceInput, enemy);
                    }
                    return true;
                case "c":
                    Console.WriteLine("You attempt to dodge!");
                    TryDodge();
                    return true;
                case "v":
                    OpenInventory();
                    bool playerHasChosen = false;
                    while (!playerHasChosen)
                    {
                        Console.WriteLine("Would you like to use an item? [Y/N]");
                        string input = (Console.ReadLine())?.ToLower();
                        if (input == "y")
                        {
                            UseInventoryInCombat(enemy);
                            playerHasChosen = true;
                        }
                        else if (input == "n")
                        {
                            Console.WriteLine("You chose not to use an item.");
                            playerHasChosen = true;
                        }
                        else
                        {
                            Console.WriteLine("You couldn't decide whether to use an item or not, so you thought about it again. [Invalid input]");
                        }
                    }
                    return true;
                case "b":
                    Focus();
                    return true;
                default:
                    Console.WriteLine("Invalid input, try again.");
                    return false;
            }
        }
        public void OpenInventory()
        {
            Console.WriteLine("INVENTORY");
            Console.WriteLine("Contents:");
            if (Inventory.Contains("Throwing knife"))
            {
                Console.WriteLine($"Throwing knife X{Inventory.Count(item => item == "Throwing knife")}");
                Console.WriteLine("  Throwable. Deals 10 damage. [Usable in combat] [One time use]");
            }
            if (Inventory.Contains("Mana potion"))
            {
                Console.WriteLine($"Mana potion X{Inventory.Count(item => item == "Mana potion")}");
                Console.WriteLine("  Restores 10 Mana upon use. [Usable in combat] [One time use]");
            }
            if (Inventory.Contains("Health potion"))
            {
                Console.WriteLine($"Health potion X{Inventory.Count(item => item == "Health potion")}");
                Console.WriteLine("  Restores 10 Health upon use. [Usable in combat] [One time use]");
            }
            if (Inventory.Contains("Brittle key"))
            {
                Console.WriteLine($"Brittle key X{Inventory.Count(item => item == "Brittle key")}");
                Console.WriteLine("  Opens a locked door. [One timme use]");
            }
            if (Inventory.Contains("Master key"))
            {
                Console.WriteLine($"Master key X{Inventory.Count(item => item == "Master key")}");
                Console.WriteLine("  Opens locked doors and never breaks.");
            }
            Console.WriteLine("");
        }
        public void UseInventoryInCombat(Enemy enemy)
        {
            if (Inventory.Count == 0)
            {
                Console.WriteLine("Your inventory is empty...");
                Console.WriteLine("");
                return;
            }
            bool chosenItemToUSe = false;
            while (!chosenItemToUSe)
            {
                Console.WriteLine("Which item would you like to use?");
                if (Inventory.Contains("Throwing knife"))
                {

                    Console.WriteLine($"[1] Use Throwing knife");
                }
                if (Inventory.Contains("Mana potion"))
                {

                    Console.WriteLine($"[2] Use Mana potion");
                }
                if (Inventory.Contains("Health potion"))
                {

                    Console.WriteLine($"[3] Use Health potion");
                }
                Console.WriteLine("");

                int.TryParse(Console.ReadLine(), out int useItemInput);
                chosenItemToUSe = makeUseItemInCombatChoice(useItemInput, enemy);
            }
        }
        public bool makeUseItemInCombatChoice(int choice, Enemy enemy)
        {
            switch (choice)
            {
                case 1:
                    if (Inventory.Contains("Throwing knife"))
                    {
                        Console.WriteLine($"You throw the knife at {enemy.name}");
                        Inventory.Remove("Throwing knife");
                        enemy.GetAttacked(10);
                        return true;
                    }
                    Console.WriteLine("You either hallucinated a Throwing knife in your inventory or fat fingered a button. [Invalid input]");
                    return false;
                case 2:
                    if (Inventory.Contains("Mana potion"))
                    {
                        Console.WriteLine("You hastily drink the potion.");
                        Inventory.Remove("Mana potion");
                        GainMana(10);
                        return true;
                    }
                    Console.WriteLine("You either tried to think a Mana potion into existence or fat fingered a button. Perhaps you've gotten addicted? [Invalid input]");
                    return false;
                case 3:
                    if (Inventory.Contains("Health potion"))
                    {
                        Console.WriteLine("You chug the potion.");
                        Inventory.Remove("Health potion");
                        Heal(10);
                        return true;
                    }
                    Console.WriteLine("You either felt VERY desperate for some health or fat fingered a button. [Invalid input]");
                    return false;
                default:
                    Console.WriteLine("Yeahh no, that option doesn't exist... [Invalid input]");
                    return false;
            }
        }
        public bool makeAttackChoice(int choice, Enemy enemy)
        {
            switch (choice)
            {
                case 1:
                    PunchAttack(enemy);
                    return true;
                case 2:
                    if(Inventory.Contains("Throwing knife"))
                    {
                        KnifeAttack(enemy);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("You either fat fingered a button or tried to slash with a knife you did not have.. [Invalid Input]");
                        return false;
                    }


                case 3:
                    if (Mana >= 7)
                    {
                        Mana -= 7;
                        CastFireBall(enemy);
                        return true;
                    }
                    else
                        Console.WriteLine("You do not have enough mana to do that!");
                    return false;
                case 4:
                    if (Mana >= 14)
                    {
                        Mana -= 14;
                        CastExplosion(enemy);
                        return true;
                    }
                    else
                        Console.WriteLine("You do not have enough mana to do that!");
                    return false;
                default:
                    Console.WriteLine("Invalid input. Try again.");
                    return false;
            }
        }
        public void PunchAttack(Enemy enemy)
        {
            int damage = 2 * meeleeAttackMultiplier;
            enemy.GetAttacked(damage);
        }

        public void KnifeAttack(Enemy enemy)
        {
            int damage = 3 * meeleeAttackMultiplier;
            enemy.GetAttacked(damage);
        }
        

        public void CastFireBall(Enemy enemy)
        {
            int damage = 3 * castSpellMultiplier;
            enemy.GetAttacked(damage);
        }

        public void CastExplosion(Enemy enemy)
        {
            int damage = 5 * castSpellMultiplier;
            enemy.GetAttacked(damage);
        }
       
        
        
        public void Focus()
        {
            GainMana(potentialManaGainedUponRoundStart);
        }
    }
    class Warrior : Player
    {
       public Warrior() : base()
        {
            maxHealth = 15;
            Health = maxHealth;
            maxMana = 7;
            Mana = 7;
            className = "Warrior";                        
        }
    }
    class Knight : Player
    {
        public Knight() : base()
        {
            maxHealth = 10;
            Health = maxHealth;
            maxMana = 10;
            Mana = 10;
            className = "Knight";
        }
    }
    class Mage : Player
    {
        public Mage() : base()
        {
            maxHealth = 5;
            Health = maxHealth;
            maxMana = 15;
            Mana = 15;
            className = "Mage";
        }
    }
    class Enemy : IHealthManager
    {
        public List<Action<Player>> attackPool = new List<Action<Player>>();
        public Enemy()
        {
            attackPool.Add(SlashAttack);
            attackPool.Add(HeavyAttack);
            name = "Enemyname";
            maxHealth = 10;
            Health = maxHealth;
            killMessage = "Generic death message.";
            introMessage = "Generic intro message.";
        }
        public string killMessage;
        public string introMessage;
        public string name;
        public int maxHealth;
        private int health;
        bool isDodgingThisTurn = false;       
        public int dodgeChance = 35;
        private Random randomizer = new Random();
        public int Health
        {
            get { return health; }
            set
            {
                if (value < 0)
                { value = 0; }
                else if (value > maxHealth)
                { value = maxHealth; }
                health = value;
            }
        }               
        public void FullHeal()
        {
            Health = maxHealth;
        }
        public void ResetHealth()
        {
            maxHealth = 10;
            Health = maxHealth;
        }
        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (amount == 0)
            {
                Console.WriteLine($"The enemy dodged your attack.");
            }
            else
            {
                Console.WriteLine($"You dealt {amount} damage to the enemy!");
            }
        }
        public void Heal(int amount)
        {
            Health += amount;
            Console.WriteLine($"The enemy has healed {amount} points of health!");            
        }
        public void GetAttacked(int amount)
        {            
            if (isDodgingThisTurn)
            {
                TakeDamage(0);
            }
            else
            {
                TakeDamage(amount);
            }            
        }
        public void ShowStats()
        {
            Console.WriteLine($"======================\nENEMY STATE\n\nTYPE: {name}\nHEALTH: {Health}/{maxHealth}\n======================");
        }
        public void UpdateDodge(bool successfullyDodgedLastTime)
        {
            if (successfullyDodgedLastTime)
            { ResetDodge(); }
            else
            { dodgeChance += 15; }
        }
        public void ResetDodge()
        {
            dodgeChance = 35;
        }

        public void TryDodge()
        {          
            int dodgingRollResult = randomizer.Next(1, 101);
            if (dodgingRollResult <= dodgeChance)
            {
                isDodgingThisTurn = true;
                Console.WriteLine($"{name} prepares to potentially dodge your attack.\n");
            }
            else
            {
                isDodgingThisTurn = false;
                Console.WriteLine($"{name} prepares to potentially dodge your attack.\n");
            }            
        }
        public void HandleTurn(Player player) 
        {
            int choiceThreshold;
            int enemyTurnChoiceResult;           
            enemyTurnChoiceResult = randomizer.Next(1, 101);
            if(dodgeChance > 35)
            {
                choiceThreshold = dodgeChance;
            }
            else
            {
                choiceThreshold = 33;
            }
            if(enemyTurnChoiceResult <= choiceThreshold)
            {
                TryDodge();
            }
            else
            {
                ChooseAttack(player);
            }            
        }
        public void ChooseAttack(Player player)
        {         
            int listIndexChoice = randomizer.Next(0, attackPool.Count);
            Action<Player> chosenAttack = attackPool[listIndexChoice];
            chosenAttack(player);         
        }
        public virtual void SlashAttack(Player player)
        {
            string attackName = "Slash";
            Console.WriteLine($"{name} performed {attackName}!");
            int damage = 2;
            player.GetAttacked(damage);
        }
        public virtual void HeavyAttack(Player player)
        {
            string attackName = "Bash";
            Console.WriteLine($"{name} performed {attackName}!");
            int damage = 5;
            player.GetAttacked(damage);
        }
        public void StartTurn(Player player) //Note to self: if i want to call this, i need to pass in "player1".
        {
            bool successfullyDodgedLastTime = isDodgingThisTurn;
            UpdateDodge(successfullyDodgedLastTime);
            isDodgingThisTurn = false;
            HandleTurn(player);
        }
    }
    class Pierceroftheheavens : Enemy
    {
        public Pierceroftheheavens()
        {
            name = "Piercer of the heavens";
            maxHealth = 9999;
            Health = maxHealth;
            attackPool.Add(Smite);
            killMessage = "You are eviscerated in an instant.";
            introMessage = "$You feel your skin glow a radiant blue as the room is swallowed by the flames of past glory coming to haunt you.\nTHE HOLY ONE descends upon you.";
        }
        public void Smite (Player player)
        {
            string attackName = "smite";
            Console.WriteLine($"{name} performed {attackName}!");
            int damage = 9999;
            player.GetAttacked(damage);
        }
    }
    class PiercerOfTheHeavensEncounter : Enemy
    {
        public PiercerOfTheHeavensEncounter()
        {
            name = "Piercer of the heavens";
            maxHealth = 60;
            Health = maxHealth;
            attackPool.Clear();
            attackPool.Add(Smite);
            attackPool.Add(Beam);
            attackPool.Add(Enlighten);
            attackPool.Add(UltraHeal);
            attackPool.Add(Punishment);
            attackPool.Add(Reduction);
            killMessage = "You are eviscerated in an instant.";
            introMessage = "The room is swallowed by flames.\nTHE HOLY ONE descends upon you.";
        }
        public void Smite(Player player)
        {
            Console.WriteLine("GOD: SMITE!");
            string attackName = "smite";
            Console.WriteLine($"{name} performed {attackName}!");
            int damage = 10;
            player.GetAttacked(damage);
        }
        public void Beam(Player player)
        {
            Console.WriteLine("GOD: REPENT!");
            string attackName = "beam";
            Console.WriteLine($"{name} performed {attackName}!");
            int damage = 2;
            player.GetAttacked(damage);
            player.GetAttacked(damage);
            player.GetAttacked(damage);
            player.GetAttacked(damage);
        }
        public void Enlighten(Player player)
        {
            Console.WriteLine("GOD: You have not earned this lifeforce!");
            string attackName = "enlighten";
            Console.WriteLine($"{name} performed {attackName}!");
            player.GetAttacked(2);
            Heal(10);
        }
        public void UltraHeal(Player player)
        {
            Console.WriteLine("GOD: YOU ARE FRAGILE!");
            string attackName = "ultraheal";
            Console.WriteLine($"{name} performed {attackName}!");
            Console.WriteLine("The enemy's max health has increased by 10!");
            maxHealth += 10;
        }
        public void Punishment(Player player)
        {
            Console.WriteLine("GOD: SINNER!");
            string attackName = "punishment";
            Console.WriteLine($"{name} performed {attackName}!");
            player.GetAttacked(8);           
        }
        public void Reduction(Player player)
        {
            Console.WriteLine("GOD: FILTH!");
            string attackName = "reduction";
            Console.WriteLine($"{name} performed {attackName}!");
            player.LoseMana(7);
        }
    }
    class Imp : Enemy
    {
        public Imp()
        {
            name = "Imp";
            maxHealth = 7;
            Health = maxHealth;
            attackPool.Clear();
            attackPool.Add(Slash);            
            killMessage = "With a quick jab, the Imp strikes your heart. Struggling to move, you are stabbed repeatedly until you collapse.";
            introMessage = "You face an enraged goblin wielding a dagger.";
        }
        public void Slash (Player player)
        {
            string attackName = "slash";
            Console.WriteLine($"{name} performed {attackName}!");
            int damage = 5;
            player.GetAttacked(damage);
        }      
    }
    class Skeleton : Enemy
    {
        public Skeleton()
        {
            name = "Skeleton";
            maxHealth = 11;
            Health = maxHealth;
            killMessage = "The skeleton grabs a rock and bashes your head into a pulp";
            introMessage = "A skeleton rises from the darkness";
        }
    }
    class LivingCorpse : Enemy
    {
        public LivingCorpse()
        {
            name = "Living corpse";
            maxHealth = 15;
            Health = maxHealth;
            killMessage = "After knocking you unconscious, the living corpse starts gnawing at your bones";
            introMessage = "Suddenly, a corpse rises from the floor";
            attackPool.Add(Chomp);
        }
        public void Chomp(Player player)
        {
            string attackName = "Chomp";
            Console.WriteLine($"{name} performed {attackName}!");
            int damage = 6;
            player.GetAttacked(damage);
        }
    }
    class BigImp : Enemy
    {
        public BigImp()
        {
            name = "Grand imp";
            maxHealth = 20;
            Health = maxHealth;
            killMessage = ("The grand imp grabs you by the legs and slams you against the floor, reducing your head to a splatter");
            introMessage = ("An unexpectedly large imp charges at you");
            attackPool.Clear();
            attackPool.Add(Punch);
            attackPool.Add(Stomp);
        }
        public void Punch(Player player)
        {
            string attackName = "Punch";
            Console.WriteLine($"{name} performed {attackName}!");
            int damage = 7;
            player.GetAttacked(damage);
        }
        public void Stomp(Player player)
        {
            string attackName = "Stomp";
            Console.WriteLine($"{name} performed {attackName}!");
            int damage = 6;
            player.GetAttacked(damage);
        }
    }
    class ImpSorcerer : Enemy
    {
        public ImpSorcerer()
        {
            name = "Imp sorcerer";
            maxHealth = 15;
            Health = maxHealth;
            killMessage = ("The imp sorcerer overwhelms you with dark, twisted magic");
            introMessage = ("An im wielding a wand is waiting for you");
            attackPool.Clear();
            attackPool.Add(MimicFireCast);
            attackPool.Add(MimicExploCast);
            attackPool.Add(Drain);
        }
        public void MimicFireCast(Player player)
        {
            Console.WriteLine($"{name} mimicks your Fireball spell!");

            player.GetAttacked(player.castSpellMultiplier * 3);
        }
        public void MimicExploCast(Player player)
        {
            Console.WriteLine($"{name} mimicks your Explosion spell!");

            player.GetAttacked(player.castSpellMultiplier * 5);
        }
        public void Drain(Player player)
        {
            Console.WriteLine($"{name} attempts to drain your mana using their dark magic!");
            player.LoseMana(5);
        }
    }
    class Room
    {
        public string? description;
        public Enemy? enemy;
        public bool hasTreasure;
        private static Dictionary<string, int> lootPool = new Dictionary<string, int>()
        {
            {"Mana potion", 20 },
            {"Brittle key", 30 },
            {"Health potion", 30 },
            {"Master key", 10 },
            {"Throwing knife", 10 }
        };
        public bool hasEnemy;
        public string treasureName;
        public bool hasDoor;
        public int numberOfDoors;
        public bool[] doorLockStates;
        public Random randomizer = new Random();
        public Room()
        {
            int totalDoors;
            totalDoors = randomizer.Next(1, 4);
            numberOfDoors = totalDoors;
            doorLockStates = new bool[numberOfDoors];
            for (int i = 0; i < numberOfDoors; i++)
            {
                doorLockStates[i] = randomizer.Next(0, 2) == 1;
            }
            hasEnemy = randomizer.Next(0, 3) != 1;
            hasTreasure = randomizer.Next(0, 2) == 1;
            if (hasTreasure)
            {
                int totalWeight = 0;
                foreach (var item in lootPool)
                {
                    totalWeight += item.Value;
                }
                int roll = randomizer.Next(0, totalWeight);
                foreach (var item in lootPool)
                {
                    if (roll < item.Value)
                    {
                        treasureName = item.Key;
                        break;
                    }
                    roll -= item.Value;
                }
            }   
        }        
        public virtual Room Enter(Player player)
        {
            Console.Clear();
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"Room {Program.roomNumber}");
            Console.WriteLine(description);
            Console.WriteLine("-----------------------------");
            if(hasEnemy)
            {
                enemy = CreateEnemy();
            }
            if (enemy != null)
            {
                Console.WriteLine("There's an enemy in the room. Prepare to fight!");
                Console.WriteLine(enemy.introMessage);
                enemy.ShowStats();

                Console.WriteLine("");
                while(true)
                {
                    if(enemy.Health < 1)
                    {
                        Console.WriteLine($"{enemy.name} died. You won!");
                        enemy = null;
                        break;
                    }
                    else if(player.Health < 1)
                    {
                        Console.WriteLine(enemy.killMessage);
                        Console.WriteLine($"You died.\nGAME OVER!");
                        Console.WriteLine("Press any button to close the application.");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    player.StartTurn(enemy);
                       
                    if(enemy.Health < 1) { continue; }
                    enemy.StartTurn(player);
                    enemy.ShowStats();

                }
            }
            Room nextRoom = null;
            string doorText = $"{(numberOfDoors > 1 ? ($"{numberOfDoors} doors") : $"a door")}";           
            string searchADoorText = $"{(numberOfDoors > 1 ? ($"[1] Try opening one of the doors") : ($"[1] Try opening the door."))}";
            while (nextRoom == null)
            {
                Console.WriteLine($"The room has {doorText}. \nWhat will you do?\n{searchADoorText}\n[2] Search the room\n");
                string input = Console.ReadLine();
                int.TryParse(input, out int choiceInput);
                nextRoom = MakeRoomChoice(player, choiceInput);
            }
            return nextRoom;
        }
        public Room MakeRoomChoice(Player player, int choice)
        {
            switch(choice)
            {
                case 1:
                    if(numberOfDoors == 1)
                    {
                        return MakeOpenDoorChoice(player, 0);                       
                    }
                    if(numberOfDoors > 1)
                    {
                        bool openDoorChoiceMade = false;
                        while(!openDoorChoiceMade)
                        {
                            Console.WriteLine($"Which one of the doors will you be attempting to open?\n[1] Quit trying to open doors");
                            for (int i = 1; i <= numberOfDoors; i++)
                            {
                                Console.WriteLine($"[{i + 1}] Open door number {i}");
                            }
                            if(int.TryParse(Console.ReadLine(), out int doorChoice))
                            {
                                if(doorChoice == 1)
                                {
                                    return null;
                                }
                                int acutalDoorSelected = doorChoice - 2;
                                if(acutalDoorSelected >= 0 && acutalDoorSelected < numberOfDoors)
                                {                                    
                                    return MakeOpenDoorChoice(player, acutalDoorSelected);
                                }
                            }
                            Console.WriteLine("You grasped at a door that didn't exist. [Invalid input]");
                        }
                        return null;                       
                    }
                    return null;
                case 2:
                    if (!hasTreasure)
                    {
                        Console.WriteLine("The room doesn't seem to contain any treasure");
                        return null;
                    }
                    Console.WriteLine($"The room has a piece of treasure [{treasureName}] lying around! Would you like to pick it up? Y/N");
                    string pickUpTreasureInput = (Console.ReadLine()).ToLower();
                    if(pickUpTreasureInput == "y")
                    {
                        PickUpTreasure(player);
                        return null;
                    }
                    else if (pickUpTreasureInput == "n")
                    {
                        Console.WriteLine($"You didn't care enough to pick up the {treasureName}.");
                        return null;
                    }
                    else
                    {
                        Console.WriteLine("You fat fingered a button, and it wasn't Y nor N.");
                        return null;
                    }                                                                           
                default:
                    return null;
            }
        }
        public Room MakeOpenDoorChoice(Player player, int choice)
        {
            int doorIndex = choice;
            if(!player.Inventory.Contains("Brittle key") && !player.Inventory.Contains("Master key"))
            {
                (doorLockStates[doorIndex]) = false;
            }           
            if(doorLockStates[doorIndex] == false)
            {
                Console.Clear();
                Console.WriteLine("You press down the handle...");
                Thread.Sleep(1000);
                Console.WriteLine("...and the door is unlocked! You enter.");
                Thread.Sleep(1000);                
                Program.roomNumber++;
                Room nextRoom = CreateRoom();                
                return nextRoom;
            }
            Console.Clear();
            Console.WriteLine("You press down the handle...");
            Thread.Sleep(1000);
            Console.WriteLine("...but the door is locked.");
            if(player.Inventory.Contains("Master key"))
            {
                Console.WriteLine("You remember you have the master key in your pocket, quickly pull it out and unlock the door.");
                doorLockStates[doorIndex] = false;
                return null;
            }
            else if (player.Inventory.Contains("Brittle key"))
            {
                Console.WriteLine($"You remember having a key that could fit this lock. Use the Brittle key? [Y/N] (You have {player.Inventory.Count(item => item == "Brittle key")} Brittle key/s.");
                Thread.Sleep(1000);
                bool hasDecided = false;
                while(!hasDecided)
                {
                    string wantsToUseKey = Console.ReadLine().ToLower();                    
                    if (wantsToUseKey == "y")
                    {
                        player.Inventory.Remove("Brittle key");
                        doorLockStates[doorIndex] = false;
                        Console.WriteLine("You manage unlock the door, but the Brittle key snaps in half. [-1 Brittle key]");
                        hasDecided = true;
                    }
                    else if (wantsToUseKey == "n")
                    {
                        Console.WriteLine("You decide to not use the key.");
                        hasDecided = true;
                    }
                    else
                    {
                        Console.WriteLine("You just stare at the door. [Invalid input]");
                    }                    
                }
                return null;                
            }
            else
            {
                Console.WriteLine("It doesn't seem like you have a way to open the door.");
                return null;
            }                        
        }
        void PickUpTreasure(Player player)
        {
            Console.WriteLine($"You pick up the {treasureName}");
            player.Inventory.Add(treasureName);
            hasTreasure = false;
        }
        public Enemy CreateEnemy()
        {
            int roll = randomizer.Next(1, 131);
            if(Program.roomNumber >= Program.bossRoomNumber)
            {
                return new PiercerOfTheHeavensEncounter();
            }
            else if(roll <= 50)
            {
                return new Imp();

            }
            else if(roll <= 85)
            {
                return new Skeleton();
            }
            else if(roll <= 99)
            {
                return new LivingCorpse();
            }
            else if(roll <= 109)
            {
                return new ImpSorcerer();
            }
            else if(roll <= 129)
            {
                return new BigImp();
            }
            else
            {
                return new Pierceroftheheavens();
            }
        }
        public Room CreateRoom()
        {
            if(Program.roomNumber >= Program.bossRoomNumber)
            {
                return new BossRoom();
            }
            int roll = randomizer.Next(1, 101);
            if(roll <= 40)
            {
                return new BasicRoom();
            }
            else if(roll <= 59)
            {
                return new HallWay();
            }
            else if(roll <= 80)
            {
                return new BarrenRoom();
            }
            else if(roll <= 87)
            {
                return new DmgRoom();
            }
            else if(roll <=92)
            {
                return new ManaRoom();
            }
            else
            {
                return new HealthRoom();
            }
        }        
    }
    class HallWay : Room
    {
        public HallWay() : base()
        {
            description = "You enter a long hallway with a single door at the end"; // heh, written on the wall with blood you see
            // top 10 anime waifus : 1. Anya 2. Eri 3. sylphiette 4. megumin 5. dawn 6. misty 7. serina 8.lopunny 9. gardevior 10 last but not leastneliel child verison  
            numberOfDoors = 1;
            doorLockStates = new bool[numberOfDoors];
            doorLockStates[0] = (randomizer.Next(0, 2) == 1);
            hasEnemy = false;
            hasTreasure = false;
        }
    }
    class ManaRoom : Room
    {
        public ManaRoom() : base()
        {
            description = $"Seems like a normal room, but something feels off.. You feel your mana potential rising... \n[You gained 5 max mana]\n[Yoir mana was replenished]\n[Your mana gain was increased]";
            hasEnemy = false;            
        }
        public override Room Enter(Player player)
        {
            player.maxMana += 5;
            player.Mana = player.maxMana;
            player.potentialManaGainedUponRoundStart += 2;
            return base.Enter(player);
        }
    }
    class HealthRoom : Room
    {
        public HealthRoom() : base()
        {
            description = $"Seems like a normal room, but something feels off.. You feel more lively...\n[Your max health has increased by 5!]\n[Your health has increased by 5!]";            
            hasEnemy = false;
        }
        public override Room Enter(Player player)
        {
            player.maxHealth += 5;
            player.FullHeal();
            return base.Enter(player);
        }
    }
    class DmgRoom : Room
    {
        public DmgRoom() : base()
        {
            description = "Seems like a normal room, but something feels off.. [You feel one of your attack types getting stronger!..";
            hasEnemy = false;
        }
        public override Room Enter(Player player)
        {
            int fireAttackUpgrade = (randomizer.Next(0, 2));
            if(fireAttackUpgrade == 1)
            {
                Console.Write(" your maana based attacks!]");
                player.castSpellMultiplier += 1;
            }
            else
            {
                Console.Write(" your meelee attacks!]");
                player.meeleeAttackMultiplier += 1;
            }
            return base.Enter(player);
        }
    }
    class BasicRoom : Room
    {
        public BasicRoom() : base()
        {
            description = "Seems like a normal room";
        }
    }
    class BarrenRoom : Room
    {
        public BarrenRoom() : base()
        {
            description = "The rooms seems to be completely barren";
            numberOfDoors = 1;
            doorLockStates = new bool[numberOfDoors];
            doorLockStates[0] = false;
            hasEnemy = false;
            hasTreasure = false;
        }
    }
    class BossRoom : Room
    {
        public BossRoom() : base()
        {
            description = ($"You feel the weight of your sins push you away from the gilded hallway, yet you keep moving.\nYou enter a large, gilded room with an empty throne.");
            hasEnemy = true;
            hasTreasure = false;
            numberOfDoors = 1;
            doorLockStates = new bool[1];
            doorLockStates[0] = false;            
        }
        public override Room Enter(Player player)
        {
            return base.Enter(player);
        }
    }
}
