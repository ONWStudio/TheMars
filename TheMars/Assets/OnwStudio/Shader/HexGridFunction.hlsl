void distance_to_segment_float(const float2 p, const float2 a, const float2 b, out float distance)
{
	const float2 ab = b - a;
	const float2 ap = p - a;

	const float ab_squared = dot(ab, ab);

	float2 closest_point;
	if (ab_squared == 0.0)
	{
		closest_point = a;
	}
	else
	{
		const float t = saturate(dot(ap, ab) / ab_squared);
		closest_point = a + t * ab;
	}

	distance = length(p - closest_point);
}

void calculate_alpha_float(
	const float2 p,
	const float2 v0, const float2 v1, const float2 v2,
	const float2 v3, const float2 v4, const float2 v5,
	const float line_width,
	out float alpha)
{
	float distances[6];
	distance_to_segment_float(p, v0, v1, distances[0]);
	distance_to_segment_float(p, v1, v2, distances[1]);
	distance_to_segment_float(p, v2, v3, distances[2]);
	distance_to_segment_float(p, v3, v4, distances[3]);
	distance_to_segment_float(p, v4, v5, distances[4]);
	distance_to_segment_float(p, v5, v0, distances[5]);
    
	float min_distance = distances[0];
	for (int i = 1; i < 6; i++)
	{
		min_distance = min(min_distance, distances[i]);
	}

	alpha = saturate(1.0 - min_distance / line_width);
}

void calculate_alpha_float(const float2 center, const float2 p, const float2 vertices[6], const float line_width_ratio, out float alpha)
{
	float distances[6];
	distance_to_segment_float(p, vertices[0], vertices[1], distances[0]);
	float min_distance = distances[0];
	for (int i = 1; i < 6; i++)
	{
		distance_to_segment_float(p, vertices[i], vertices[(i + 1) % 6u], distances[i]);
		min_distance = min(min_distance, distances[i]);
	}

	alpha = saturate(1.0 - min_distance / line_width_ratio);
}

void get_hex_corner_float2(const float2 center, const float radius, const int index, out float2 corner)
{
	const float pi = 3.14159265359f;
	const float angle_deg = 60 * index;
	const float angle_rad = pi / 180.0f * angle_deg;

	corner = float2(
		center.x + radius * cos(angle_rad),
		center.y + radius * sin(angle_rad));
}

void calculate_hex_outline_float(const float2 hex_pixel, const float2 now_pixel, const float radius, const float line_width, out float alpha)
{
	float2 vertices[6];

	for (int i = 0; i < 6; i++)
	{
		get_hex_corner_float2(hex_pixel, radius, i, vertices[i]);
	}
	
	calculate_alpha_float(hex_pixel, now_pixel, vertices, line_width, alpha);
}