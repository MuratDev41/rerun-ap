from BaseClasses import Region, Location, Item, Tutorial
from worlds.AutoWorld import World, WebWorld

from .items import item_table
from .locations import location_table
from .Options import rerun_options, RerunOptions


class RerunItem(Item):
	game = "RE:RUN"


class RerunLocation(Location):
	game = "RE:RUN"


class RerunWebWorld(WebWorld):
	theme = "ocean"
	tutorials = [Tutorial(
		"Multiworld Setup Guide",
		"A guide to setting up RE:RUN with Archipelago.",
		"English",
		"setup_en.md",
		"setup/en",
		["MuratDeveloper"]
	)]


class RerunWorld(World):
	"""RE:RUN is a fast-paced platformer by Dani where you must reach the finish line before
	time runs out across 10 increasingly difficult levels."""

	game = "RE:RUN"
	web = RerunWebWorld()
	topology_present = True
	options_dataclass = RerunOptions
	options = rerun_options

	item_name_to_id = {
		name: data.code
		for name, data in item_table.items()
	}

	location_name_to_id = {
		name: data.code
		for name, data in location_table.items()
	}

	def create_item(self, name: str) -> RerunItem:
		data = item_table[name]
		return RerunItem(name, data.classification, data.code, self.player)

	def create_items(self) -> None:
		itempool = []

		level_unlocks = [
			f"Level {i} Unlock"
			for i in range(0, 11)
		]

		# Pick one random level unlock to start with
		starting_level_unlock = self.random.choice(level_unlocks)

		# Start with it, and remove it from the pool
		self.multiworld.push_precollected(self.create_item(starting_level_unlock))

		# Progression items
		itempool.append(self.create_item("Sword"))
		itempool.append(self.create_item("Double Jump"))
		itempool.append(self.create_item("Rewind"))

		# Add every level unlock EXCEPT the starting one
		for unlock in level_unlocks:
			if unlock != starting_level_unlock:
				itempool.append(self.create_item(unlock))

		# Fill every remaining location with filler
		location_count = len(location_table)

		while len(itempool) < location_count:
			itempool.append(self.create_item("Milk"))

		self.multiworld.itempool += itempool
		
	def create_regions(self) -> None:
		menu = Region("Menu", self.player, self.multiworld)
		self.multiworld.regions.append(menu)

		regions = {}

		for loc_data in location_table.values():
			region_name = loc_data.region

			if region_name not in regions:
				region = Region(region_name, self.player, self.multiworld)
				regions[region_name] = region
				self.multiworld.regions.append(region)
				menu.connect(region)

		for loc_name, loc_data in location_table.items():
			region = regions[loc_data.region]
			location = RerunLocation(self.player, loc_name, loc_data.code, region)
			region.locations.append(location)

	def set_rules(self) -> None:
		from .Rules import set_rules
		set_rules(self)

	def get_filler_item_name(self) -> str:
		return "Milk"

	def fill_slot_data(self) -> dict:
		slot_data = {
			"death_link": self.options.death_link.value,
			"death_link_amnesty": self.options.death_link_amnesty.value,
		}
		
		import logging
		logging.info(f"[RE:RUN] Generated slot data: {slot_data}")
		
		return slot_data