from worlds.generic.Rules import set_rule

LOCATION_RULES = {
	# Level completion checks
	"Level 0 Completed": ["Sword", "Double Jump", "Level 0 Unlock"],
	"Level 1 Completed": ["Sword", "Double Jump", "Level 1 Unlock"],
	"Level 2 Completed": ["Sword", "Double Jump", "Level 2 Unlock"],
	"Level 3 Completed": ["Sword", "Double Jump", "Level 3 Unlock"],
	"Level 4 Completed": ["Rewind", "Double Jump", "Level 4 Unlock"],
	"Level 5 Completed": ["Double Jump", "Level 5 Unlock"],
	"Level 6 Completed": ["Level 6 Unlock"],
	"Level 7 Completed": ["Rewind", "Sword", "Level 7 Unlock"],
	"Level 8 Completed": ["Double Jump", "Level 8 Unlock"],
	"Level 9 Completed": ["Double Jump", "Level 9 Unlock"],
	"Level 10 Completed": ["Double Jump", "Level 10 Unlock"],

	# Non-level-completion checks
	"Level 0 - Sword": ["Double Jump", "Level 0 Unlock"],
	"Level 0 - Double Jump": ["Level 0 Unlock"],

	"Level 1 - Sword": ["Level 1 Unlock"],
	"Level 1 - Double Jump": ["Sword", "Level 1 Unlock"],

	"Level 2 - Sword": ["Level 2 Unlock"],
	"Level 2 - Double Jump": ["Sword", "Level 2 Unlock"],

	"Level 3 - Sword": ["Double Jump", "Level 3 Unlock"],
	"Level 3 - Double Jump": ["Level 3 Unlock"],

	"Level 4 - Rewind": ["Level 4 Unlock"],
	"Level 4 - Double Jump": ["Rewind", "Level 4 Unlock"],

	"Level 5 - Double Jump": ["Level 5 Unlock"],

	"Level 7 - Rewind": ["Sword", "Level 7 Unlock"],
	"Level 7 - Sword": ["Level 7 Unlock"],

	"Level 8 - Double Jump": ["Level 8 Unlock"],
	"Level 9 - Double Jump": ["Level 9 Unlock"],
	"Level 10 - Double Jump": ["Level 10 Unlock"],

	# Enemy Checks
	"Level 0 - The Poor Swordsman": ["Level 0 Unlock", "Double Jump", "Sword"],
	"Level 1 - Blue Double Jump Archer": ["Level 1 Unlock", "Sword"],
	"Level 2 - The red swordman right at spawn": ["Level 2 Unlock", "Sword"],
	"Level 2 - The blue Double Jump Swordsman": ["Level 2 Unlock", "Sword"],
	"Level 3 - The Blue Archer Across The Way": ["Level 3 Unlock"],
	"Level 3 - The Red Swordman before barricade": ["Level 3 Unlock", "Double Jump", "Sword"],
	"Level 3 - The Red Swordman Right at the end": ["Level 3 Unlock", "Double Jump", "Sword"],
	"Level 4 - The Red Challanger 1": ["Level 4 Unlock"],
	"Level 4 - The Red Challanger 2": ["Level 4 Unlock"],
	"Level 4 - The Blue Challanger": ["Level 4 Unlock"],
	"Level 4 - The Blue Archer On Top": ["Level 4 Unlock", "Rewind"],
	"Level 4 - The Red Archer On Top": ["Level 4 Unlock", "Rewind"],
	"Level 5 - The Red Enemy That Is Aproaching": ["Level 5 Unlock"],
	"Level 5 - The Annoying Red Archer On The Back": ["Level 5 Unlock"],
	"Level 5 - The Watcher Swordman 1": ["Level 5 Unlock"],
	"Level 5 - The Watcher Swordman 2": ["Level 5 Unlock"],
	"Level 5 - The Blue King Archer": ["Level 5 Unlock"],
	"Level 6 - The Giant Blue Swordman At Start": ["Level 6 Unlock"],
	"Level 6 - The Suprise Archer 1": ["Level 6 Unlock"],
	"Level 6 - The Suprise Archer 2": ["Level 6 Unlock"],
	"Level 6 - The Suprise Archer 3": ["Level 6 Unlock"],
	"Level 6 - The Suprise Archer 4": ["Level 6 Unlock"],
	"Level 7 - The Red Archer in front": ["Level 7 Unlock", "Sword"],
	"Level 7 - The Red Swordman on Top": ["Level 7 Unlock", "Sword"],
	"Level 7 - The Red Swordman in the Box 1": ["Level 7 Unlock", "Sword"],
	"Level 7 - The Red Swordman in the Box 2": ["Level 7 Unlock", "Sword"],
	"Level 7 - The Red Archer at the End": ["Level 7 Unlock", "Rewind", "Sword"],
	"Level 7 - The Red Swordman at the End": ["Level 7 Unlock", "Rewind", "Sword"],
	"Level 8 - The Red Swordman after sliding": ["Level 8 Unlock"],
	"Level 8 - The Blue Archer after that": ["Level 8 Unlock"],
	"Level 8 - The Red Swordman after Double Jump": ["Level 8 Unlock", "Double Jump"],
	"Level 8 - The Red Swordman after The Red Swordman after Double Jump": ["Level 8 Unlock", "Double Jump"],
	"Level 8 - The Red Swordman after The Red Swordman after The Red Swordman after Double Jump": ["Level 8 Unlock", "Double Jump"],
	"Level 9 - The Red Swordman at the start 1": ["Level 9 Unlock"],
	"Level 9 - The Red Swordman at the start 2": ["Level 9 Unlock"],
	"Level 9 - The Red Archer at the top": ["Level 9 Unlock"],
	"Level 9 - The Red Archer at the elevator": ["Level 9 Unlock"],
	"Level 9 - The Blue Giant Swordman waiting for the elevator": ["Level 9 Unlock"],
	"Level 9 - The Red Archer that annoys me so much that i want to end him right there right now": ["Level 9 Unlock", "Double Jump"],
	"Level 10 - The Red Swordman in the Room": ["Level 10 Unlock"],
	"Level 10 - The Red Archer at the Castle 1": ["Level 10 Unlock"],
	"Level 10 - The Red Archer at the Castle 2": ["Level 10 Unlock"],
	"Level 10 - The Blue Archer at the Castle": ["Level 10 Unlock"],
	"Level 10 - The Red Swordman at the end": ["Level 10 Unlock", "Double Jump"],
	"Level 10 - The Red Archer at the end": ["Level 10 Unlock", "Double Jump"],
}


def make_item_rule(world, items):
	player = world.player

	return lambda state: all(
		state.has(item, player)
		for item in items
	)


def set_rules(world) -> None:
	for loc_name, items in LOCATION_RULES.items():
		location = world.multiworld.get_location(loc_name, world.player)
		set_rule(location, make_item_rule(world, items))

	required_completion_locations = [
		"Level 0 Completed",
		"Level 1 Completed",
		"Level 2 Completed",
		"Level 3 Completed",
		"Level 4 Completed",
		"Level 5 Completed",
		"Level 6 Completed",
		"Level 7 Completed",
		"Level 8 Completed",
		"Level 9 Completed",
		"Level 10 Completed",
	]

	world.multiworld.completion_condition[world.player] = lambda state: all(
		state.can_reach(loc_name, "Location", world.player)
		for loc_name in required_completion_locations
	)