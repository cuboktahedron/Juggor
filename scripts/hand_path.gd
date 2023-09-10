class_name HandPath
extends Path2D

var time := 1.0


func _init(name: String, time: float, points: Array):
	self.name = name
	self.time = time
	
	curve = Curve2D.new()
	for point in points:
		curve.add_point(point.position, point["in"], point.out)
	
	var path_follow := PathFollow2D.new()
	path_follow.name = "%sPathFollow" % name
	path_follow.loop = false

	add_child(path_follow)
	
