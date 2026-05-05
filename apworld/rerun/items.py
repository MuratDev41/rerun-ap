from typing import Dict
from BaseClasses import ItemClassification
from dataclasses import dataclass

BASE_ID = 3310000

@dataclass
class RerunItemData:
    code: int
    classification: ItemClassification

item_table: Dict[str, RerunItemData] = {
    # Powerup items - locked behind Archipelago
    "Sword":           RerunItemData(BASE_ID + 301, ItemClassification.progression),
    "Double Jump":     RerunItemData(BASE_ID + 302, ItemClassification.progression),
    "Rewind":          RerunItemData(BASE_ID + 303, ItemClassification.progression),
    # Level Locks
    "Level 0 Unlock":           RerunItemData(BASE_ID + 101, ItemClassification.progression),
    "Level 1 Unlock":           RerunItemData(BASE_ID + 102, ItemClassification.progression),
    "Level 2 Unlock":           RerunItemData(BASE_ID + 103, ItemClassification.progression),
    "Level 3 Unlock":           RerunItemData(BASE_ID + 104, ItemClassification.progression),
    "Level 4 Unlock":           RerunItemData(BASE_ID + 105, ItemClassification.progression),
    "Level 5 Unlock":           RerunItemData(BASE_ID + 106, ItemClassification.progression),
    "Level 6 Unlock":           RerunItemData(BASE_ID + 107, ItemClassification.progression),
    "Level 7 Unlock":           RerunItemData(BASE_ID + 108, ItemClassification.progression),
    "Level 8 Unlock":           RerunItemData(BASE_ID + 109, ItemClassification.progression),
    "Level 9 Unlock":           RerunItemData(BASE_ID + 110, ItemClassification.progression),
    "Level 10 Unlock":           RerunItemData(BASE_ID + 111, ItemClassification.progression),
    # Filler
    "Milk":            RerunItemData(BASE_ID + 201, ItemClassification.filler),
}
