extends PopupMenu

signal change_pattern(patterns: Array)
signal change_pattern_toggled(is_checked: bool)

const MENU_ID_CHANGE_PATTERN = 0
const MENU_ID_3 = 1
const MENU_ID_4 = 2
const MENU_ID_1234567 = 3
const MENU_ID_5 = 4


func _on_id_pressed(id):
	if id == MENU_ID_CHANGE_PATTERN:
		toggle_item_checked(MENU_ID_CHANGE_PATTERN)
		change_pattern_toggled.emit(is_item_checked(MENU_ID_CHANGE_PATTERN))
	elif id == MENU_ID_3:
		change_pattern.emit([3])
	elif id == MENU_ID_4:
		change_pattern.emit([4])
	elif id == MENU_ID_1234567:
		change_pattern.emit([1, 2, 3, 4, 5, 6, 7])
	elif id == MENU_ID_5:
		change_pattern.emit([5])


func _on_change_pattern_form_change_pattern(Array):
	set_item_checked(MENU_ID_CHANGE_PATTERN, false)
