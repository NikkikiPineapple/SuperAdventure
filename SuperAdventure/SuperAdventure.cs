using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Engine;
using static System.Windows.Forms.Design.AxImporter;
using System.Numerics;

namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        private Player _player;
        private Monster _currentMonster;
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";

        public SuperAdventure()
        {
            InitializeComponent();
            if (File.Exists(PLAYER_DATA_FILE_NAME))
            {
                _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
            }
            else
            {
                _player = Player.CreateDefaultPlayer();
            }
            MoveTo(_player.CurrentLocation);
            UpdatePlayerStats();
            UpdateCraftingListInUI();


        }

        private void cboWeapons_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player.CurrentWeapon = (Weapon)cboWeapons.SelectedItem;
        }

        private void cboRecipes_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player.CurrentRecipe = (Recipe)cboRecipes.SelectedItem;
        }

        private void UpdatePlayerStats()
        {
            // Refresh player information and inventory controls
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }

        private void checkForCraftingRecipes()
        {
            // loop through each item in the players inventory
            foreach (InventoryItem ii in _player.Inventory.ToList())
            {
                // check to see if the user has materials in their inventory
                if (ii.Details.ID == 14)
                {
                    // check to see if the user has enough of the materials
                    if (ii.Quantity >= 5)
                    {
                        // remove required materials from the player
                        ii.Quantity -= 5;
                        // give the player the crafted item
                        _player.AddItemToInventory((World.ItemByID(15)));

                        // Refresh player's crafting combobox
                        UpdateWeaponListInUI();

                        rtbMessages.Text += Environment.NewLine + "You have crafted successfully! " + Environment.NewLine;
                        ScrollToBottomOfMessages();



                    }
                    else
                    {
                        rtbMessages.Text += Environment.NewLine + "not enough materials." + Environment.NewLine;
                        ScrollToBottomOfMessages();
                    }

                }
            }
        }

        private void ScrollToBottomOfMessages()
        {
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void MoveTo(Location newLocation)
        {
            //Does the location have any required items
            if (!_player.HasRequiredItemToEnterThisLocation(newLocation))
            {
                rtbMessages.Text += "You must have a " + newLocation.ItemRequiredToEnter.Name + " to enter this location." + Environment.NewLine;
                ScrollToBottomOfMessages();
                return;
            }

            // Update the player's current location
            _player.CurrentLocation = newLocation;

            // Show/hide available movement buttons
            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);

            // Display current location name and description
            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;

            // Completely heal the player
            _player.CurrentHitPoints = _player.MaximumHitPoints;

            // Update Hit Points in UI
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();

            // Does the location have a quest?
            if (newLocation.QuestAvailableHere != null)
            {
                // See if the player already has the quest, and if they've completed it
                bool playerAlreadyHasQuest = _player.HasThisQuest(newLocation.QuestAvailableHere);
                bool playerAlreadyCompletedQuest = _player.CompletedThisQuest(newLocation.QuestAvailableHere);

                // See if the player already has the quest
                if (playerAlreadyHasQuest)
                {
                    // If the player has not completed the quest yet
                    if (!playerAlreadyCompletedQuest)
                    {
                        // See if the player has all the items needed to complete the quest
                        bool playerHasAllItemsToCompleteQuest = _player.HasAllQuestCompletionItems(newLocation.QuestAvailableHere);

                        // The player has all items required to complete the quest
                        if (playerHasAllItemsToCompleteQuest)
                        {
                            // Display message
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "You complete the '" + newLocation.QuestAvailableHere.Name + "' quest." + Environment.NewLine;
                            ScrollToBottomOfMessages();

                            // Remove quest items from inventory
                            _player.RemoveQuestCompletionItems(newLocation.QuestAvailableHere);

                            // Give quest rewards
                            rtbMessages.Text += "You receive: " + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardGold.ToString() + " gold" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;
                            ScrollToBottomOfMessages();

                            _player.AddExperiencePoints(newLocation.QuestAvailableHere.RewardExperiencePoints);
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            // Add the reward item to the player's inventory
                            _player.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);

                            // Mark the quest as completed
                            _player.MarkQuestCompleted(newLocation.QuestAvailableHere);
                        }
                    }
                }
                else
                {
                    // The player does not already have the quest

                    // Display the messages
                    rtbMessages.Text += Environment.NewLine + "You receive the " + newLocation.QuestAvailableHere.Name + " quest." + Environment.NewLine + Environment.NewLine;

                    rtbMessages.Text += newLocation.QuestAvailableHere.Description + Environment.NewLine;
                    rtbMessages.Text += Environment.NewLine + "To complete it, return with:" + Environment.NewLine;
                    ScrollToBottomOfMessages();
                    foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                    {
                        if (qci.Quantity == 1)
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.NamePlural + Environment.NewLine;
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;
                    ScrollToBottomOfMessages();

                    // Add the quest to the player's quest list
                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

            // Does the location have a monster?
            if (newLocation.MonsterLivingHere != null)
            {
                rtbMessages.Text += "You see a " + newLocation.MonsterLivingHere.Name + Environment.NewLine;
                ScrollToBottomOfMessages();

                // Make a new monster, using the values from the standard monster in the World.Monster list
                Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);

                _currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage,
                    standardMonster.RewardExperiencePoints, standardMonster.RewardGold, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints);

                foreach (LootItem lootItem in standardMonster.LootTable)
                {
                    _currentMonster.LootTable.Add(lootItem);
                }

                cboWeapons.Visible = true;
                cboPotions.Visible = true;
                btnUseWeapon.Visible = true;
                btnUsePotion.Visible = true;
            }
            else
            {
                _currentMonster = null;

                cboWeapons.Visible = false;
                cboPotions.Visible = false;
                btnUseWeapon.Visible = false;
                btnUsePotion.Visible = false;
            }

            // Refresh player's inventory list
            UpdateInventoryListInUI();

            // Refresh player's quest list
            UpdateQuestListInUI();

            // Refresh player's weapons combobox
            UpdateWeaponListInUI();

            // Refresh player's potions combobox
            UpdatePotionListInUI();
        }

        private void UpdateInventoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;

            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";

            dgvInventory.Rows.Clear();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { inventoryItem.Details.Name, inventoryItem.Quantity.ToString() });
                }
            }
        }

        private void UpdateQuestListInUI()
        {
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";

            dgvQuests.Rows.Clear();

            foreach (PlayerQuest playerQuest in _player.Quests)
            {
                dgvQuests.Rows.Add(new[] { playerQuest.Details.Name, playerQuest.IsCompleted.ToString() });
            }
        }

        private void UpdateCraftingListInUI()
        {
            List<Recipe> recipes = new List<Recipe>();

            foreach (InventoryRecipe x in _player.Recipes)
            {
                if (x.Details is Recipe)
                {
                    if (x.Quantity > 0)
                    {
                        recipes.Add((Recipe)x.Details);
                    }
                }
            }

            if (recipes.Count == 0)
            {
                // The player doesn't have any weapons, so hide the weapon combobox and "Use" button
                cboRecipes.Visible = false;
                btnCraftItem.Visible = false;
            }
            else
            {
                cboRecipes.SelectedIndexChanged -= cboRecipes_SelectedIndexChanged;
                cboRecipes.DataSource = recipes;
                cboRecipes.SelectedIndexChanged += cboRecipes_SelectedIndexChanged;
                cboRecipes.DisplayMember = "Name";
                cboRecipes.ValueMember = "ID";

                if (_player.CurrentRecipe != null)
                {
                    cboRecipes.SelectedItem = _player.CurrentRecipe;
                }
                else
                {
                    cboRecipes.SelectedIndex = 0;
                }
            }
        }

        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is Weapon)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.Details);
                    }
                }
            }

            if (weapons.Count == 0)
            {
                // The player doesn't have any weapons, so hide the weapon combobox and "Use" button
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.SelectedIndexChanged -= cboWeapons_SelectedIndexChanged;
                cboWeapons.DataSource = weapons;
                cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";

                if (_player.CurrentWeapon != null)
                {
                    cboWeapons.SelectedItem = _player.CurrentWeapon;
                }
                else
                {
                    cboWeapons.SelectedIndex = 0;
                }
            }
        }

        private void UpdatePotionListInUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is HealingPotion)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)inventoryItem.Details);
                    }
                }
            }

            if (healingPotions.Count == 0)
            {
                // The player doesn't have any potions, so hide the potion combobox and "Use" button
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";

                cboPotions.SelectedIndex = 0;
            }
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            // Get the currently selected weapon from the cboWeapons ComboBox
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

            // Determine the amount of damage to do to the monster
            int damageToMonster = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);

            // Apply the damage to the monster's CurrentHitPoints
            _currentMonster.CurrentHitPoints -= damageToMonster;

            // Display message
            rtbMessages.Text += "You hit the " + _currentMonster.Name + " for " + damageToMonster.ToString() + " points." + Environment.NewLine;
            ScrollToBottomOfMessages();

            // Check if the monster is dead
            if (_currentMonster.CurrentHitPoints <= 0)
            {
                // Monster is dead
                rtbMessages.Text += Environment.NewLine;
                rtbMessages.Text += "You defeated the " + _currentMonster.Name + Environment.NewLine;
                ScrollToBottomOfMessages();

                // Give player experience points for killing the monster
                _player.AddExperiencePoints(_currentMonster.RewardExperiencePoints);
                rtbMessages.Text += "You receive " + _currentMonster.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;

                // Give player gold for killing the monster 
                _player.Gold += _currentMonster.RewardGold;
                rtbMessages.Text += "You receive " + _currentMonster.RewardGold.ToString() + " gold" + Environment.NewLine;
                ScrollToBottomOfMessages();

                // Get random loot items from the monster
                List<InventoryItem> lootedItems = new List<InventoryItem>();

                // Add items to the lootedItems list, comparing a random number to the drop percentage
                foreach (LootItem lootItem in _currentMonster.LootTable)
                {
                    if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                    {
                        lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                    }
                }

                // If no items were randomly selected, then add the default loot item(s).
                if (lootedItems.Count == 0)
                {
                    foreach (LootItem lootItem in _currentMonster.LootTable)
                    {
                        if (lootItem.IsDefaultItem)
                        {
                            lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                        }
                    }
                }

                // Add the looted items to the player's inventory
                foreach (InventoryItem inventoryItem in lootedItems)
                {
                    _player.AddItemToInventory(inventoryItem.Details);

                    if (inventoryItem.Quantity == 1)
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.Name + Environment.NewLine;
                        ScrollToBottomOfMessages();
                    }
                    else
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.NamePlural + Environment.NewLine;
                        ScrollToBottomOfMessages();
                    }
                }

                // Refresh player information and inventory controls
                lblHitPoints.Text = _player.CurrentHitPoints.ToString();
                lblGold.Text = _player.Gold.ToString();
                lblExperience.Text = _player.ExperiencePoints.ToString();
                lblLevel.Text = _player.Level.ToString();

                UpdateInventoryListInUI();
                UpdateWeaponListInUI();
                UpdatePotionListInUI();

                // Add a blank line to the messages box, just for appearance.
                rtbMessages.Text += Environment.NewLine;
                ScrollToBottomOfMessages();

                // Move player to current location (to heal player and create a new monster to fight)
                MoveTo(_player.CurrentLocation);
            }
            else
            {
                // Monster is still alive

                // Determine the amount of damage the monster does to the player
                int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);

                // Display message
                rtbMessages.Text += "The " + _currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine;
                ScrollToBottomOfMessages();

                // Subtract damage from player
                _player.CurrentHitPoints -= damageToPlayer;

                // Refresh player data in UI
                lblHitPoints.Text = _player.CurrentHitPoints.ToString();

                if (_player.CurrentHitPoints <= 0)
                {
                    // Display message
                    rtbMessages.Text += "The " + _currentMonster.Name + " killed you." + Environment.NewLine;
                    ScrollToBottomOfMessages();

                    // Move player to "Home"
                    MoveTo(World.LocationByID(World.LOCATION_ID_START_CAVE));
                }
            }
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            // Get the currently selected potion from the combobox
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;

            // Add healing amount to the player's current hit points
            _player.CurrentHitPoints = (_player.CurrentHitPoints + potion.AmountToHeal);

            // CurrentHitPoints cannot exceed player's MaximumHitPoints
            if (_player.CurrentHitPoints > _player.MaximumHitPoints)
            {
                _player.CurrentHitPoints = _player.MaximumHitPoints;
            }

            // Remove the potion from the player's inventory
            foreach (InventoryItem ii in _player.Inventory)
            {
                if (ii.Details.ID == potion.ID)
                {
                    ii.Quantity--;
                    break;
                }
            }

            // Display message
            rtbMessages.Text += "You drink a " + potion.Name + Environment.NewLine;
            ScrollToBottomOfMessages();

            // Monster gets their turn to attack

            // Determine the amount of damage the monster does to the player
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);

            // Display message
            rtbMessages.Text += "The " + _currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine;
            ScrollToBottomOfMessages();

            // Subtract damage from player
            _player.CurrentHitPoints -= damageToPlayer;

            if (_player.CurrentHitPoints <= 0)
            {
                // Display message
                rtbMessages.Text += "The " + _currentMonster.Name + " killed you." + Environment.NewLine;
                ScrollToBottomOfMessages();

                // Move player to "Home"
                MoveTo(World.LocationByID(World.LOCATION_ID_START_CAVE));
            }

            // Refresh player data in UI
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            UpdateInventoryListInUI();
            UpdatePotionListInUI();
        }

        private void SuperAdventure_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXmlString());
        }



        private void buyBoneSword_Click(object sender, EventArgs e)
        {
            rtbMessages.Text += Environment.NewLine + "5 bones are required." + Environment.NewLine;
            ScrollToBottomOfMessages();

            // loop through each item in the players inventory
            foreach (InventoryItem ii in _player.Inventory.ToList())
            {
                // check to see if the user has bones in their inventory
                if (ii.Details.ID == 14)
                {
                    // check to see if the user has 5 bones
                    if (ii.Quantity >= 5)
                    {
                        // remove 5 bones from the player
                        ii.Quantity -= 5;
                        // give the player a bone sword
                        _player.AddItemToInventory((World.ItemByID(15)));

                        // Refresh player's weapons combobox
                        UpdateWeaponListInUI();

                        rtbMessages.Text += Environment.NewLine + "Bone Sword added to inventory! " + Environment.NewLine;
                        ScrollToBottomOfMessages();



                    }
                    else
                    {
                        rtbMessages.Text += Environment.NewLine + "5 bones are required." + Environment.NewLine;
                        ScrollToBottomOfMessages();
                    }

                }
            }
            // update inventory list
            UpdateInventoryListInUI();

        }

        private void craftFireSword_Click(object sender, EventArgs e)
        {
            rtbMessages.Text += Environment.NewLine + "1 Fire rune is required." + Environment.NewLine;
            ScrollToBottomOfMessages();
            // loop through each item in the players inventory
            foreach (InventoryItem ii in _player.Inventory.ToList())
            {
                // check to see if the user has a fire rune in their inventory
                if (ii.Details.ID == 16)
                {
                    // check to see if the user has 1 fire rune
                    if (ii.Quantity >= 1)
                    {

                        // give the player a fire sword
                        _player.AddItemToInventory((World.ItemByID(18)));
                        // remove 1 fire rune from the player
                        ii.Quantity -= 1;

                        // Refresh player's weapons combobox
                        UpdateWeaponListInUI();

                        rtbMessages.Text += Environment.NewLine + "Fire Sword added to inventory! " + Environment.NewLine + Environment.NewLine;
                        ScrollToBottomOfMessages();


                    }
                    else
                    {
                        rtbMessages.Text += Environment.NewLine + "1 fire rune is required." + Environment.NewLine;
                        ScrollToBottomOfMessages();
                    }


                }

            }
            // update inventory list
            UpdateInventoryListInUI();
        }

        private void craftLeatherHelmet_Click(object sender, EventArgs e)
        {
            rtbMessages.Text += Environment.NewLine + "1 Fire rune is required." + Environment.NewLine;
            ScrollToBottomOfMessages();
            // loop through each item in the players inventory
            foreach (InventoryItem x in _player.Inventory.ToList())
            {
                // check to see if the user has a fire rune in their inventory
                if (x.Details.ID == 16)
                {
                    // check to see if the user has 1 fire rune
                    if (x.Quantity >= 1)
                    {

                        // give the player a fire sword
                        _player.AddItemToInventory((World.ItemByID(18)));
                        // remove 1 fire rune from the player
                        x.Quantity -= 1;

                        // Refresh player's weapons combobox
                        UpdateWeaponListInUI();

                        rtbMessages.Text += Environment.NewLine + "Fire Sword added to inventory! " + Environment.NewLine + Environment.NewLine;
                        ScrollToBottomOfMessages();


                    }
                    else
                    {
                        rtbMessages.Text += Environment.NewLine + "1 fire rune is required." + Environment.NewLine;
                        ScrollToBottomOfMessages();
                    }


                }

            }
            // update inventory list
            UpdateInventoryListInUI();
        }

        private void btnCraftItem_Click(object sender, EventArgs e)
        {

            // Get selected item from UI

            if (_player.CurrentRecipe != null)
            {
                cboRecipes.SelectedItem = _player.CurrentRecipe;
            }
            else
            {
                cboRecipes.SelectedIndex = 0;
            }


            

            ScrollToBottomOfMessages();


            // Loop through player recipes
            foreach (InventoryRecipe x in _player.Recipes.ToList())
            {
                // check to see if player has recipe
                if (x.Details.Name == cboRecipes.Text)
                {
                    // loop throgh players inventory
                    foreach (InventoryItem ii in _player.Inventory.ToList())
                    {
                        // check if player has the required crafting item
                        if (ii.Details.ID == x.Details.RequiredItemID)
                        {
                            
                            if (ii.Quantity >=  x.Details.RequiredAmount)
                            {

                                // give the player the item
                                _player.AddItemToInventory((World.ItemByID(x.Details.CraftedItemID)));
                                // remove item(s) from the player
                                ii.Quantity -= x.Details.RequiredAmount;

                                rtbMessages.Text += Environment.NewLine + "You have successfully crafted: " + cboRecipes.Text + Environment.NewLine;
                                ScrollToBottomOfMessages();
                                //update UI
                                UpdateCraftingListInUI();
                                UpdateWeaponListInUI();
                                UpdatePotionListInUI();
                                UpdateInventoryListInUI();
                            } 
                            else
                            {
                                rtbMessages.Text += Environment.NewLine + "you do not have enough items to make: " + cboRecipes.Text + Environment.NewLine;
                                ScrollToBottomOfMessages();
                            }
                            
                        } 

                    }
                }

            }

        }
    }
}