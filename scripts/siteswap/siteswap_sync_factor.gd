class_name SiteswapSyncFactor

var _ss_height1: SiteswapSyncHeight
var _ss_height2: SiteswapSyncHeight
var is_cross1: bool
var is_cross2: bool


var height1:
	get: return _ss_height1.height
var height2:
	get: return _ss_height2.height

func _init(
	height1: SiteswapSyncHeight,
	is_cross1: bool,
	height2: SiteswapSyncHeight,
	is_cross2: bool):
	self._ss_height1 = height1
	self.is_cross1 = is_cross1
	self._ss_height2 = height2
	self.is_cross2 = is_cross2


static func parse(context: SiteswapParseContext) -> SiteswapSyncFactor:
	var c = context.peek()
	
	if c == "(":
		context.next()

		var height1 = SiteswapSyncHeight.parse(context)
		if height1 == null:
			return null
			
		if context.next() != ",":
			return null
			
		var height2 = SiteswapSyncHeight.parse(context)
		if height2 == null:
			return null
		
		if context.next() != ")":
			return  null
			
		return SiteswapSyncFactor.new(
			height1,
			false,
			height2,
			false)
	else:
		return null


func label():
	return "(%s, %s)" % [_ss_height1.label(), _ss_height2.label()]
