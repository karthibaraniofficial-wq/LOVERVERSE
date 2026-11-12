# LOVEVERSE - API Reference

## Standard Response Format
```json
{
  "success": true,
  "data": {},
  "error": null,
  "timestamp": "2026-11-12T03:00:00.000Z"
}
```

## Error Codes
- `UNAUTHORIZED`: Token missing or invalid.
- `ROOM_NOT_FOUND`: Room ID or code does not exist.
- `ROOM_FULL`: Maximum players reached (2 for private).
- `FORBIDDEN_MODIFICATION`: Not an owner of the world or resource.
- `VALIDATION_FAILED`: Payload schema validation failed.
