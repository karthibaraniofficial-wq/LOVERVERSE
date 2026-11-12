# LOVEVERSE - Database Schema (Cloud Firestore)

### `users/{userId}`
```typescript
interface UserProfile {
  userId: string;
  displayName: string;
  avatarPresetId: string;
  appearanceData: {
    hairStyleId: string;
    hairColorHex: string;
    eyeColorHex: string;
    outfitTopId: string;
    outfitBottomId: string;
  };
  createdAt: FirebaseFirestore.Timestamp;
  lastLogin: FirebaseFirestore.Timestamp;
  accountStatus: 'active' | 'suspended';
}
```

### `rooms/{roomId}`
```typescript
interface RoomSession {
  roomId: string;
  invitationCode: string; // 6-char cryptographically secure
  ownerId: string;
  memberIds: string[];
  maxCapacity: number; // 2 for private
  createdAt: FirebaseFirestore.Timestamp;
  expiresAt: FirebaseFirestore.Timestamp;
  status: 'open' | 'active' | 'closed';
}
```

### `worlds/{worldId}`
```typescript
interface SharedWorld {
  worldId: string;
  ownerIds: string[];
  currentRegion: string;
  timeOfDayMinutes: number;
  weatherState: string;
  furniture: Array<{
    instanceId: string;
    itemId: string;
    position: [number, number, number];
    rotationEuler: [number, number, number];
  }>;
  updatedAt: FirebaseFirestore.Timestamp;
}
```

### `memories/{memoryId}`
```typescript
interface SharedMemory {
  memoryId: string;
  worldId: string;
  createdBy: string;
  storageUrl: string;
  caption: string;
  locationId: string;
  createdAt: FirebaseFirestore.Timestamp;
  visibility: 'private' | 'shared';
}
```
