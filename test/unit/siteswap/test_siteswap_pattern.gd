extends GutTest


func test_is_valid_returns_true_if_pattern_is_valid():
	assert_true(SiteswapPattern.parse("0").is_valid())
	assert_true(SiteswapPattern.parse("3").is_valid())
	assert_true(SiteswapPattern.parse("Z").is_valid())
	assert_true(SiteswapPattern.parse("345").is_valid())
	assert_true(SiteswapPattern.parse(" 3 4 5 ").is_valid())
	assert_true(SiteswapPattern.parse("123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ").is_valid())
	assert_true(SiteswapPattern.parse("(2, 4)").is_valid())
	assert_true(SiteswapPattern.parse("(2, 4)(2, 4)").is_valid())


func test_parse_returns_null_if_parse_failed():
	assert_null(SiteswapPattern.parse("$"))
	assert_null(SiteswapPattern.parse("a"))
	assert_null(SiteswapPattern.parse("(3, 3)"))


func test_is_valid_returns_false_if_pattern_is_invalid():
	assert_false(SiteswapPattern.parse("223").is_valid())
	assert_false(SiteswapPattern.parse("54321").is_valid())
	assert_false(SiteswapPattern.parse("123456789ABCDEFGHIJKLMNOPQRSTUVWXY").is_valid())
	assert_false(SiteswapPattern.parse("(2, 4)(4, 2)").is_valid())


func test_ball_num_returns_ball_num_of_pattern_if_pattern_is_valid():
	assert_eq(SiteswapPattern.parse("0").ball_num(), 0)
	assert_eq(SiteswapPattern.parse("3").ball_num(), 3)
	assert_eq(SiteswapPattern.parse("Z").ball_num(), 35)
	assert_eq(SiteswapPattern.parse("345").ball_num(), 4)
	assert_eq(SiteswapPattern.parse("(2, 4)").ball_num(), 3)


func test_ball_num_returns_0_if_pattern_is_invalid():
	assert_eq(SiteswapPattern.parse("223").ball_num(), 0)
	assert_eq(SiteswapPattern.parse("54321").ball_num(), 0)
	assert_eq(SiteswapPattern.parse("123456789ABCDEFGHIJKLMNOPQRSTUVWXY").ball_num(), 0)
	assert_eq(SiteswapPattern.parse("(2, 4)(4, 2)").ball_num(), 0)


func test_label_returns_label_of_siteswap():
	assert_eq(SiteswapPattern.parse("9AB").label(), "9AB")


func test_max_height_returns_max_height_of_pattern_if_pattern_is_valid():
	assert_eq(SiteswapPattern.parse("12345").max_height(), 5)
	assert_eq(SiteswapPattern.parse("(2, 4)").max_height(), 4)


func test_max_height_returns_0_if_pattern_is_invalid():
	assert_eq(SiteswapPattern.parse("223").max_height(), 0)
	assert_eq(SiteswapPattern.parse("(2, 4)(4, 2)").max_height(), 0)

