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
    # Filler
    "Milk":            RerunItemData(BASE_ID + 201, ItemClassification.filler),
}
