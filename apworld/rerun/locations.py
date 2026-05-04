from typing import Dict
from dataclasses import dataclass

BASE_ID = 3310000

@dataclass
class RerunLocationData:
    code: int
    region: str

location_table: Dict[str, RerunLocationData] = {
    # Level completion locations (one per level)
    "Level 0 Completed":  RerunLocationData(BASE_ID + 1,  "Level 0"),
    "Level 1 Completed":  RerunLocationData(BASE_ID + 2,  "Level 1"),
    "Level 2 Completed":  RerunLocationData(BASE_ID + 3,  "Level 2"),
    "Level 3 Completed":  RerunLocationData(BASE_ID + 4,  "Level 3"),
    "Level 4 Completed":  RerunLocationData(BASE_ID + 5,  "Level 4"),
    "Level 5 Completed":  RerunLocationData(BASE_ID + 6,  "Level 5"),
    "Level 6 Completed":  RerunLocationData(BASE_ID + 7,  "Level 6"),
    "Level 7 Completed":  RerunLocationData(BASE_ID + 8,  "Level 7"),
    "Level 8 Completed":  RerunLocationData(BASE_ID + 9,  "Level 8"),
    "Level 9 Completed": RerunLocationData(BASE_ID + 10, "Level 9"),
    "Level 10 Completed": RerunLocationData(BASE_ID + 11, "Level 10"),
}
