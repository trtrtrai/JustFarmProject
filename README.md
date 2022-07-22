# JustFarmProject
The farm game
Line up:
_ Day 1: 23/05
PlantCell, Plant, Growing() (of PlantCell).
_ Day 2: 24/05
Click to place crop, harvest.
_ Day 3: 25/05
Tool board.
Click to select PlantSeed, object follow pointer.
Double click to deselect tool.
_ Day 4: 26/05
Growing() (private in Plant), independent grow by StartCoroutine().
Update harvest action.
Setup PlantInCell and PlantPerCell.
_ Day 5: 27/05
Item, , interface IItem, Item<IItem>.
ItemObject.
Storage (Inventory) (MonoBehaviour).
_ Day 6: 28/05
Update Item<IItem>, interface IItem, etc.
Storage Mono.
Storage Canvas: scroll view, item button, ItemCellButton,  ItemCell Mono script.
update namespace.
Product item.
CanvasGrid.
_ Day 7: 30/05
AddItem to storage, UI.
Harvest crop to storage, change amout, new item if it max.
StorageFilter UI, Script, FilterBar.
_ Day 8: 31/05
Fillter storage(seed/product...) code.
ItemDetails UI.
Money Canvas.
_ Day 9: 02/06
Event click ItemCell to active sell details.
(Img, name, description, Action of slider-inputField-UpDownArrow, Change price sell real time).
Update class Money (Sprite, Name).
_ Day 10: 03/06
DataLoader static class.
Save/Load currency.
_ Day 11: 04/06
Sell item => money (event money change).
Reduce amount while sell item, update inventory, details UI.
Destroy item if amount == 0.
Update new item prefabs, item data.
Shop UI.
_ Day 12: 05/06
Shop data, save load itemShop data.
_ Day 13: 06/06
Save load itemShop object, infomation, slider,...
_ Day 14: 07/06
Buy item => money check, update itemShop data, update storage.
_ Day 15: 08/06
Update Tool behaviour (sub seed while plant).
ToolPage UI, Synchronise storage - ToolManager - ToolObj infomation.
_ Day 16: 11/06
Sell all item -> destroy but details UI don't disappeared (solved).
Buy item limit don't decrease on UI, while limit ==0 click buy still buy item to storage with amount = 0 (solve).
Clear itemShop data if item limit==0 (solve).
_ Day 17: 12/06
Save planting data (crop, state, timer,...) by 3 class serializeable: CropData, PlantCellData, PlantData.
Load planting data: deserialize, PlantCell self Load, new func overload in Plant.
_ Day 18: 14/06
Lock plant cell cannot growing plant.
Nofication UI, Nofication class, code chain.
Unlock plantcell (by money) => show dialog => accept-check => (15/6) result-resolve.
Class Manager to stay GameState resolve bug: harvest crop while open inventory, shop,...
_ Day 19: 15/06
Update SubMoney (reference) to check enough => TrySubMoney.
Completed yesterday's work and add message while buy item, sell item.
_ Day 20: 16/06
Class Manager change GameState to [Stack].
Sort capacity (compile item) (EXTEND) (trick i find in my code is save and load data => sort same items don't get max).
Disable buy button in shop while value or max item can buy equal to 0.
Exception capacity storage full while buy -> can not buy and open notify.
_ Day 21: 18/06
Canvas Scaler to auto rezise obj.
Handle Tool UI: shovel, hoe. Action in PlantCell script.
Update EmptySoil script to sync with data and tool.
_ Day 22: 19/06
Decor tile palette, add new creature comming soon (fishing, fish farm), new crop wheat, ingredents potion craft,...
Update button shop, inventory rect.
Update Money container, bg.
_ Day 23: 20/06
AudioManager class, get audio source, music background, update play sound every action, message.
Setting canvas UI, slider sound/music UI.
_ Day 24: 22/06
Setting button, event nav, value change slider.
Home scene decor, menu, event button PLAY.
_ Day 25: 23/06
Moving Home UI Canvas (animation).
Don't destroy on load to loadscene + save status to load in next scene like sound, music,...
_ Day 26: 24/06
Change mouse image.
UI update: message box, storage, shop, tool select.
_ Day 27: 26/06
Add img to set cursor symbol ↖️.
Build game: fail, cannot load data??? Children canvas??, Start()?, serialize dataPath???
_ Day 28: 22/07
Resolved build data: StreamingAssetsPath for .txt.
Need to update creature:
Save don't destroy data (volume...) (EXTEND).
Timing reset shop, chest per day to week, month.
Productivity decrease every harvest action.
Multiple plant once plant cell, upgrade plant cell (EXTEND).
Exception capacity full while get prize (i think send to mail is resolve).
Image resize sprite in CS6 (resolution failed): you need to find image vector or big image to make next game ;)).
Infomation around.
User select, user folder (EXTEND).
Friends (EXTEND).
Resolution window (EXTEND).
Cross platform (EXTEND).
