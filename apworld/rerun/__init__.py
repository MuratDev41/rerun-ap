from worlds.AutoWorld import World, WebWorld
from BaseClasses import Region, Item, ItemClassification, Tutorial
from .items import item_table, RerunItemData
from .locations import location_table

BASE_ID = 3310000


class RerunItem(Item):
    game = "RE:RUN"


class RerunWebWorld(WebWorld):
    theme = "ocean"
    tutorials = [Tutorial(
        "Multiworld Setup Guide",
        "A guide to setting up RE:RUN with Archipelago.",
        "English",
        "setup_en.md",
        "setup/en",
        ["YourName"]
    )]


class RerunWorld(World):
    """RE:RUN is a fast-paced platformer by Dani where you must reach the finish line before
    time runs out across 10 increasingly difficult levels."""

    game = "RE:RUN"
    web = RerunWebWorld()
    topology_present = True

    item_name_to_id = {name: data.code for name, data in item_table.items()}
    location_name_to_id = {name: data.code for name, data in location_table.items()}

    def create_item(self, name: str) -> RerunItem:
        data = item_table[name]
        return RerunItem(name, data.classification, data.code, self.player)

    def create_items(self) -> None:
        # Add powerups
        items = ["Sword", "Double Jump", "Rewind"]
        for item_name in items:
            self.multiworld.itempool.append(self.create_item(item_name))
        
        # Fill the rest with Speed Boosts
        total_locations = len(self.location_name_to_id)
        while len(self.multiworld.itempool) < total_locations:
            self.multiworld.itempool.append(self.create_item("Speed Boost"))

    def create_regions(self) -> None:
        menu = Region("Menu", self.player, self.multiworld)
        self.multiworld.regions.append(menu)

        for level_num in range(1, 11):
            region_name = f"Level {level_num}"
            region = Region(region_name, self.player, self.multiworld)
            self.multiworld.regions.append(region)

            location_name = f"Level {level_num} Completed"
            loc_data = location_table[location_name]
            from BaseClasses import Location

            class RerunLocation(Location):
                game = "RE:RUN"

            loc = RerunLocation(self.player, location_name, loc_data.code, region)
            region.locations.append(loc)

        # Connect Menu -> Level 1 freely (Level 1 is always accessible)
        menu.connect(self.multiworld.get_region("Level 1", self.player))

        # Connect levels in a line (no items required)
        for level_num in range(1, 10):
            src = self.multiworld.get_region(f"Level {level_num}", self.player)
            dst = self.multiworld.get_region(f"Level {level_num + 1}", self.player)
            src.connect(dst)

    def set_rules(self) -> None:
        # Win condition: complete level 10
        self.multiworld.completion_condition[self.player] = \
            lambda state: state.can_reach("Level 10 Completed", "Location", self.player)

    def get_filler_item_name(self) -> str:
        return "Speed Boost"
