extends PopupMenu

signal change_pattern(sp: SiteswapPattern)
signal change_pattern_toggled(is_checked: bool)

const MENU_ID_CHANGE_PATTERN = 0
const MENU_ID_3 = 1
const MENU_ID_4 = 2
const MENU_ID_5 = 3
const MENU_ID_1234567 = 4


func _on_id_pressed(id):
	if id == MENU_ID_CHANGE_PATTERN:
		toggle_item_checked(MENU_ID_CHANGE_PATTERN)
		change_pattern_toggled.emit(is_item_checked(MENU_ID_CHANGE_PATTERN))
	elif id == MENU_ID_3:
		change_pattern.emit(SiteswapPattern.parse("3"))
	elif id == MENU_ID_4:
		change_pattern.emit(SiteswapPattern.parse("4"))
	elif id == MENU_ID_5:
		change_pattern.emit(SiteswapPattern.parse("5"))
	elif id == MENU_ID_1234567:
		change_pattern.emit(SiteswapPattern.parse("1234567"))


func _on_change_pattern_form_change_pattern(_sp: SiteswapPattern):
	set_item_checked(MENU_ID_CHANGE_PATTERN, false)
