export interface UserProfile {
  userId: string;
  displayName: string;
  avatarPresetId: string;
  appearanceJson: string;
  createdAt: string;
  lastLogin: string;
}

export interface RoomSession {
  roomId: string;
  invitationCode: string;
  ownerId: string;
  memberIds: string[];
  status: 'open' | 'active' | 'closed';
  createdAt: string;
}

export interface SharedWorld {
  worldId: string;
  ownerIds: string[];
  currentRegion: string;
  timeOfDayHours: number;
  weather: string;
  furniture: Array<{
    instanceId: string;
    itemId: string;
    position: [number, number, number];
    rotationEuler: [number, number, number];
  }>;
  updatedAt: string;
}

export interface SharedMemory {
  memoryId: string;
  worldId: string;
  createdBy: string;
  caption: string;
  locationId: string;
  storageUrl: string;
  createdAt: string;
}

// In-Memory Repository for development & CI testing
export class DataRepository {
  private users = new Map<string, UserProfile>();
  private rooms = new Map<string, RoomSession>();
  private worlds = new Map<string, SharedWorld>();
  private memories = new Map<string, SharedMemory>();

  constructor() {
    // Seed default world for testing
    this.worlds.set('world_demo', {
      worldId: 'world_demo',
      ownerIds: ['user_demo_1', 'user_demo_2'],
      currentRegion: 'LoveTown',
      timeOfDayHours: 17.5,
      weather: 'clear',
      furniture: [],
      updatedAt: new Date().toISOString()
    });
  }

  // Users
  async getUser(userId: string): Promise<UserProfile | null> {
    return this.users.get(userId) || null;
  }

  async saveUser(user: UserProfile): Promise<void> {
    this.users.set(user.userId, user);
  }

  // Rooms
  async createRoom(room: RoomSession): Promise<void> {
    this.rooms.set(room.roomId, room);
  }

  async getRoom(roomId: string): Promise<RoomSession | null> {
    return this.rooms.get(roomId) || null;
  }

  async getRoomByCode(code: string): Promise<RoomSession | null> {
    for (const room of this.rooms.values()) {
      if (room.invitationCode.toUpperCase() === code.toUpperCase() && room.status !== 'closed') {
        return room;
      }
    }
    return null;
  }

  async updateRoom(room: RoomSession): Promise<void> {
    this.rooms.set(room.roomId, room);
  }

  // Worlds
  async getWorld(worldId: string): Promise<SharedWorld | null> {
    return this.worlds.get(worldId) || null;
  }

  async saveWorld(world: SharedWorld): Promise<void> {
    this.worlds.set(world.worldId, world);
  }

  // Memories
  async addMemory(memory: SharedMemory): Promise<void> {
    this.memories.set(memory.memoryId, memory);
  }

  async getMemoriesByWorld(worldId: string): Promise<SharedMemory[]> {
    return Array.from(this.memories.values()).filter(m => m.worldId === worldId);
  }
}

export const repository = new DataRepository();
