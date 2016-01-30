using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SandboxRPG
{
     class ItemSlot
     {
          public String itemName;
          public Texture2D itemTexture;

          public bool hasItem = false;

          public void LoadContent(ContentManager Content)
          {
               itemTexture = Content.Load<Texture2D>("items/" + itemName);
          }
     }

     class Inventory : DrawableGameComponent
     {
          private List<ItemSlot> items = new List<ItemSlot>();

          // Number of item slots without any expansion items
          private int numberOfInventorySlots = 45;

          // Max number of horizontal slots
          private int maxHorizontalSlots = 9;

          // The width of the inventory slot
          private int inventoryWidth;

          // The height of the inventory slot
          private int inventoryHeight;

          // Number of pixels between the inventory slots
          private int slotPadding = 2;
          
          // Should show the full inventory
          private bool showInventory = false;

          // Texture for the item slot
          private Texture2D itemSlotTexture;

          // Base position for the inventory
          private Vector2 inventoryStartPosition = new Vector2(10, 10);

          // Offset for the item to position it in the right spot
          private Vector2 itemOffset = new Vector2(7, 10);

          // Current item clicked by mouse
          private ItemSlot heldItem;

          private SpriteBatch spriteBatch;

          public Inventory(Game game) :
               base(game)
          {
          }

          public override void Initialize()
          {
               spriteBatch = new SpriteBatch(Game.GraphicsDevice);

               base.Initialize();
          }

          protected override void LoadContent()
          {
               itemSlotTexture = Game.Content.Load<Texture2D>("item_slot");

               inventoryWidth = itemSlotTexture.Width;
               inventoryHeight = itemSlotTexture.Height;

               // Add item slots based on the base amount of inventory slots
               for (int i = 0; i < numberOfInventorySlots; i++)
               {
                    ItemSlot item = new ItemSlot();
                    items.Add(item);
               }

               base.LoadContent();
          }

          MouseState oldMouseState;
          KeyboardState oldKeyBoardState;
          public override void Update(GameTime gameTime)
          {
               KeyboardState newKeyBoardState = Keyboard.GetState();
               MouseState newMouseState = Mouse.GetState();

               #region Keyboard input
               if (newKeyBoardState.IsKeyDown(Keys.Escape) && oldKeyBoardState.IsKeyUp(Keys.Escape))
               {
                    showInventory = !showInventory;
               }

               if (newKeyBoardState.IsKeyDown(Keys.G) && oldKeyBoardState.IsKeyUp(Keys.G))
               {
                    AddItem("shortSword");
               }
               #endregion

               #region Mouse input

               if(newMouseState != oldMouseState)
               {
                    //Debug.WriteLine("Mouse Moved");

                    if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                    {
                         int slotIndex = GetSlotIndexAt(newMouseState.X, newMouseState.Y);

                         if(heldItem == null)
                         {
                              heldItem = items[slotIndex];
                         }
                    }
               }

               #endregion

               oldMouseState = newMouseState;
               oldKeyBoardState = newKeyBoardState;

               base.Update(gameTime);
          }

          public override void Draw(GameTime gameTime)
          {
               spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

               if(showInventory)
               {
                    for (int i = 0; i < items.Count; i++)
                    {
                         Vector2 itemPosition = GetItemPosition(i);

                         if (items[i].itemTexture != null)
                         {
                              spriteBatch.Draw(items[i].itemTexture, itemPosition + itemOffset, Color.White);
                         }

                         spriteBatch.Draw(itemSlotTexture, itemPosition, Color.White);
                    }
               }
               else
               {
                    for (int x = 0; x < maxHorizontalSlots; x++)
                    {
                         Vector2 itemPosition = GetItemPosition(x);

                         if (items[x].itemTexture != null)
                         {
                              spriteBatch.Draw(items[x].itemTexture, itemPosition + itemOffset, Color.White);
                         }
                         spriteBatch.Draw(itemSlotTexture, itemPosition, Color.White);
                    }
               }

               spriteBatch.End();

               base.Draw(gameTime);
          }

          public void AddItem(String itemName, int index = 0)
          {
               if(index == 0)
               {
                    for (int i = 0; i < items.Count; i++)
                         if (!items[i].hasItem)
                         {
                              index = i;
                              break;
                         }
               }

               items[index].hasItem = true;
               items[index].itemName = itemName;
               items[index].LoadContent(Game.Content);
          }

          private void PlaceItem(int index)
          {

          }

          private ItemSlot GetSlotAt(int x, int y)
          {
               Rectangle mouseRect = new Rectangle(x, y, 2, 2);

               for (int i = 0; i < items.Count; i++)
               {
                    Vector2 itemPosition = GetItemPosition(i);
                    Rectangle slotRect = new Rectangle((int)itemPosition.X, (int)itemPosition.Y, inventoryWidth, inventoryHeight);

                    if (mouseRect.Intersects(slotRect))
                    {
                         return items[i];
                    }
               }

               return null;
          }

          private int GetSlotIndexAt(int x, int y)
          {
               Rectangle mouseRect = new Rectangle(x, y, 2, 2);

               for (int i = 0; i < items.Count; i++)
               {
                    Vector2 itemPosition = GetItemPosition(i);
                    Rectangle slotRect = new Rectangle((int)itemPosition.X, (int)itemPosition.Y, inventoryWidth, inventoryHeight);

                    if (mouseRect.Intersects(slotRect))
                    {
                         return i;
                    }
               }

               return -1;
          }

          private Vector2 GetItemPosition(int index)
          {
               Vector2 position = inventoryStartPosition;

               int x = index % maxHorizontalSlots;
               int y = index / maxHorizontalSlots;

               int xOffset = (x * (inventoryWidth + slotPadding));
               int yOffset = (y * (inventoryHeight + slotPadding));

               position += new Vector2(xOffset, yOffset);

               return position;
          }
     }
}