extends PopupMenu

signal coloring_changed(coloring: Core.BallColoring)

const MENU_ID_COLORING_BY_PATTERN = 1000
const MENU_ID_COLORING_BY_BALL_NO = 1001


func _on_id_pressed(id):
	for i in item_count:
		set_item_checked(i, false)
	set_item_checked(get_item_index(id), true)

	if id == MENU_ID_COLORING_BY_PATTERN:
		coloring_changed.emit(Core.BallColoring.BY_PATTERN)
		pass
	elif id == MENU_ID_COLORING_BY_BALL_NO:
		coloring_changed.emit(Core.BallColoring.BY_BALL_NO)
		pass
