from Options import Choice, Range, Toggle, DeathLink

class DeathLinkAmnesty(Range):
    """Amount of deaths allowed before a death link is sent."""
    display_name = "Death Link Amnesty"
    range_start = 1
    range_end = 30
    default = 0

rerun_options = {
    "death_link": DeathLink,
    "death_link_amnesty": DeathLinkAmnesty,
}
