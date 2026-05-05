from Options import Choice, Range, Toggle, DeathLink, PerGameCommonOptions
from dataclasses import dataclass

class DeathLinkAmnesty(Range):
    """Amount of deaths allowed before a death link is sent."""
    display_name = "Death Link Amnesty"
    range_start = 0
    range_end = 30
    default = 0

@dataclass
class RerunOptions(PerGameCommonOptions):
    death_link: DeathLink
    death_link_amnesty: DeathLinkAmnesty

rerun_options = {
    "death_link": DeathLink,
    "death_link_amnesty": DeathLinkAmnesty,
}
