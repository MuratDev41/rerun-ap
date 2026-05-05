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
    # Level 0
    "Level 0 - Sword":        RerunLocationData(BASE_ID + 101, "Level 0"),
    "Level 0 - Double Jump":  RerunLocationData(BASE_ID + 201, "Level 0"),
    # Level 1
    "Level 1 - Sword":        RerunLocationData(BASE_ID + 102, "Level 1"),
    "Level 1 - Double Jump":  RerunLocationData(BASE_ID + 202, "Level 1"),
    # Level 2
    "Level 2 - Sword":        RerunLocationData(BASE_ID + 103, "Level 2"),
    "Level 2 - Double Jump":  RerunLocationData(BASE_ID + 203, "Level 2"),
    # Level 3
    "Level 3 - Sword":        RerunLocationData(BASE_ID + 104, "Level 3"),
    "Level 3 - Double Jump":  RerunLocationData(BASE_ID + 204, "Level 3"),
    # Level 4
    "Level 4 - Rewind":       RerunLocationData(BASE_ID + 305, "Level 4"),
    "Level 4 - Double Jump":  RerunLocationData(BASE_ID + 205, "Level 4"),
    # Level 5
    "Level 5 - Double Jump":  RerunLocationData(BASE_ID + 206, "Level 5"),
    # Level 7
    "Level 7 - Rewind":       RerunLocationData(BASE_ID + 308, "Level 7"),
    "Level 7 - Sword":        RerunLocationData(BASE_ID + 108, "Level 7"),
    # Level 8
    "Level 8 - Double Jump":  RerunLocationData(BASE_ID + 209, "Level 8"),
    # Level 9
    "Level 9 - Double Jump":  RerunLocationData(BASE_ID + 210, "Level 9"),
    # Level 10
    "Level 10 - Double Jump": RerunLocationData(BASE_ID + 211, "Level 10"),

    # Enemy Checks
    "The Poor Swordsman": RerunLocationData(BASE_ID + 1000, "Level 0"),
    "Blue Double Jump Archer": RerunLocationData(BASE_ID + 1001, "Level 1"),
    "The red swordman right at spawn": RerunLocationData(BASE_ID + 1002, "Level 2"),
    "The blue Double Jump Swordsman": RerunLocationData(BASE_ID + 1003, "Level 2"),
    "The Blue Archer Across The Way": RerunLocationData(BASE_ID + 1004, "Level 3"),
    "The Red Swordman before barricade": RerunLocationData(BASE_ID + 1005, "Level 3"),
    "The Red Swordman Right at the end": RerunLocationData(BASE_ID + 1006, "Level 3"),
    "The Red Challanger 1": RerunLocationData(BASE_ID + 1007, "Level 4"),
    "The Red Challanger 2": RerunLocationData(BASE_ID + 1008, "Level 4"),
    "The Blue Challanger": RerunLocationData(BASE_ID + 1009, "Level 4"),
    "The Blue Archer On Top": RerunLocationData(BASE_ID + 1010, "Level 4"),
    "The Red Archer On Top": RerunLocationData(BASE_ID + 1011, "Level 4"),
    "The Red Enemy That Is Aproaching": RerunLocationData(BASE_ID + 1012, "Level 5"),
    "The Annoying Red Archer On The Back": RerunLocationData(BASE_ID + 1013, "Level 5"),
    "The Watcher Swordman 1": RerunLocationData(BASE_ID + 1014, "Level 5"),
    "The Watcher Swordman 2": RerunLocationData(BASE_ID + 1015, "Level 5"),
    "The Blue King Archer": RerunLocationData(BASE_ID + 1016, "Level 5"),
    "The Giant Blue Swordman At Start": RerunLocationData(BASE_ID + 1017, "Level 6"),
    "The Suprise Archer 1": RerunLocationData(BASE_ID + 1018, "Level 6"),
    "The Suprise Archer 2": RerunLocationData(BASE_ID + 1019, "Level 6"),
    "The Suprise Archer 3": RerunLocationData(BASE_ID + 1020, "Level 6"),
    "The Suprise Archer 4": RerunLocationData(BASE_ID + 1021, "Level 6"),
    "The Red Archer in front": RerunLocationData(BASE_ID + 1022, "Level 7"),
    "The Red Swordman on Top": RerunLocationData(BASE_ID + 1023, "Level 7"),
    "The Red Swordman in the Box 1": RerunLocationData(BASE_ID + 1024, "Level 7"),
    "The Red Swordman in the Box 2": RerunLocationData(BASE_ID + 1025, "Level 7"),
    "The Red Archer at the End": RerunLocationData(BASE_ID + 1026, "Level 7"),
    "The Red Swordman at the End": RerunLocationData(BASE_ID + 1027, "Level 7"),
    "The Red Swordman after sliding": RerunLocationData(BASE_ID + 1028, "Level 8"),
    "The Blue Archer after that": RerunLocationData(BASE_ID + 1029, "Level 8"),
    "The Red Swordman after Double Jump": RerunLocationData(BASE_ID + 1030, "Level 8"),
    "The Red Swordman after The Red Swordman after Double Jump": RerunLocationData(BASE_ID + 1031, "Level 8"),
    "The Red Swordman after The Red Swordman after The Red Swordman after Double Jump": RerunLocationData(BASE_ID + 1032, "Level 8"),
    "The Red Swordman at the start 1": RerunLocationData(BASE_ID + 1033, "Level 9"),
    "The Red Swordman at the start 2": RerunLocationData(BASE_ID + 1034, "Level 9"),
    "The Red Archer at the top": RerunLocationData(BASE_ID + 1035, "Level 9"),
    "The Red Archer at the elevator": RerunLocationData(BASE_ID + 1036, "Level 9"),
    "The Blue Giant Swordman waiting for the elevator": RerunLocationData(BASE_ID + 1037, "Level 9"),
    "The Red Archer that annoys me so much that i want to end him right there right now": RerunLocationData(BASE_ID + 1038, "Level 9"),
    "The Red Swordman in the Room": RerunLocationData(BASE_ID + 1039, "Level 10"),
    "The Red Archer at the Castle 1": RerunLocationData(BASE_ID + 1040, "Level 10"),
    "The Red Archer at the Castle 2": RerunLocationData(BASE_ID + 1041, "Level 10"),
    "The Blue Archer at the Castle": RerunLocationData(BASE_ID + 1042, "Level 10"),
    "The Red Swordman at the end": RerunLocationData(BASE_ID + 1043, "Level 10"),
    "The Red Archer at the end": RerunLocationData(BASE_ID + 1044, "Level 10"),
}
