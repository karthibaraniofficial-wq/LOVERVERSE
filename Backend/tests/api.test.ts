import { describe, it, expect, beforeAll, afterAll } from 'vitest';
import { buildServer } from '../src/server';
import { FastifyInstance } from 'fastify';

describe('LOVEVERSE Backend API Integration Tests', () => {
  let app: FastifyInstance;

  beforeAll(async () => {
    app = buildServer();
    await app.ready();
  });

  afterAll(async () => {
    await app.close();
  });

  it('GET /health returns status ok', async () => {
    const res = await app.inject({
      method: 'GET',
      url: '/health'
    });
    expect(res.statusCode).toBe(200);
    expect(res.json().status).toBe('ok');
  });

  it('GET /api/v1/openapi.json returns valid openapi schema', async () => {
    const res = await app.inject({
      method: 'GET',
      url: '/api/v1/openapi.json'
    });
    expect(res.statusCode).toBe(200);
    expect(res.json().openapi).toBe('3.0.3');
  });

  it('Rejects unauthenticated requests with 401 UNAUTHORIZED', async () => {
    const res = await app.inject({
      method: 'GET',
      url: '/api/v1/users/me'
    });
    expect(res.statusCode).toBe(401);
    expect(res.json().error).toContain('UNAUTHORIZED');
  });

  it('POST /api/v1/auth/session initializes a new user profile', async () => {
    const res = await app.inject({
      method: 'POST',
      url: '/api/v1/auth/session',
      headers: {
        authorization: 'Bearer dev-user:player_test_1'
      }
    });
    expect(res.statusCode).toBe(200);
    const body = res.json();
    expect(body.success).toBe(true);
    expect(body.data.userId).toBe('player_test_1');
  });

  it('POST /api/v1/rooms creates a private room with 6-character code', async () => {
    const res = await app.inject({
      method: 'POST',
      url: '/api/v1/rooms',
      headers: {
        authorization: 'Bearer dev-user:player_test_1'
      }
    });
    expect(res.statusCode).toBe(200);
    const body = res.json();
    expect(body.success).toBe(true);
    expect(body.data.invitationCode).toHaveLength(6);
  });

  it('POST /api/v1/rooms/join allows partner to join with valid code', async () => {
    const createRes = await app.inject({
      method: 'POST',
      url: '/api/v1/rooms',
      headers: {
        authorization: 'Bearer dev-user:player_test_1'
      }
    });
    const room = createRes.json().data;

    const joinRes = await app.inject({
      method: 'POST',
      url: '/api/v1/rooms/join',
      headers: {
        authorization: 'Bearer dev-user:player_test_2'
      },
      payload: {
        invitationCode: room.invitationCode
      }
    });
    expect(joinRes.statusCode).toBe(200);
    const joinBody = joinRes.json();
    expect(joinBody.data.memberIds).toContain('player_test_2');
    expect(joinBody.data.status).toBe('active');
  });

  it('POST /api/v1/rooms/join rejects non-existent room code with 404', async () => {
    const joinRes = await app.inject({
      method: 'POST',
      url: '/api/v1/rooms/join',
      headers: {
        authorization: 'Bearer dev-user:player_test_2'
      },
      payload: {
        invitationCode: 'XXXXXX'
      }
    });
    expect(joinRes.statusCode).toBe(404);
    expect(joinRes.json().error).toBe('ROOM_NOT_FOUND');
  });

  it('GET and POST /api/v1/inventory/transactions executes validated unlock', async () => {
    const invRes = await app.inject({
      method: 'GET',
      url: '/api/v1/inventory',
      headers: {
        authorization: 'Bearer dev-user:player_test_1'
      }
    });
    expect(invRes.statusCode).toBe(200);
    expect(invRes.json().data.loveCoins).toBeGreaterThanOrEqual(50);

    const txRes = await app.inject({
      method: 'POST',
      url: '/api/v1/inventory/transactions',
      headers: {
        authorization: 'Bearer dev-user:player_test_1'
      },
      payload: {
        itemId: 'outfit_dress_spring_01',
        category: 'outfit',
        costCoins: 30
      }
    });
    expect(txRes.statusCode).toBe(200);
    expect(txRes.json().data.unlockedOutfits).toContain('outfit_dress_spring_01');
  });

  it('POST and GET /api/v1/memories creates and retrieves photo memories', async () => {
    const createMem = await app.inject({
      method: 'POST',
      url: '/api/v1/memories',
      headers: {
        authorization: 'Bearer dev-user:player_test_1'
      },
      payload: {
        worldId: 'world_demo',
        caption: 'Sunset together in Romantic Garden',
        locationId: 'RomanticGarden_01',
        storageUrl: 'https://storage.googleapis.com/loveverse/memories/photo_01.png'
      }
    });
    expect(createMem.statusCode).toBe(200);
    expect(createMem.json().data.caption).toBe('Sunset together in Romantic Garden');

    const getMem = await app.inject({
      method: 'GET',
      url: '/api/v1/memories/world_demo',
      headers: {
        authorization: 'Bearer dev-user:player_test_1'
      }
    });
    expect(getMem.statusCode).toBe(200);
    expect(getMem.json().data.length).toBeGreaterThanOrEqual(1);
  });

  it('PATCH /api/v1/worlds/world_demo updates persistent world layout', async () => {
    const patchRes = await app.inject({
      method: 'PATCH',
      url: '/api/v1/worlds/world_demo',
      headers: {
        authorization: 'Bearer dev-user:player_test_1'
      },
      payload: {
        timeOfDayHours: 19.5,
        weather: 'rain'
      }
    });
    expect(patchRes.statusCode).toBe(200);
    expect(patchRes.json().data.timeOfDayHours).toBe(19.5);
    expect(patchRes.json().data.weather).toBe('rain');
  });

  it('POST /api/v1/reports submits report for moderation review', async () => {
    const reportRes = await app.inject({
      method: 'POST',
      url: '/api/v1/reports',
      headers: {
        authorization: 'Bearer dev-user:player_test_1'
      },
      payload: {
        reportedUserId: 'bad_actor',
        reason: 'Harassment',
        details: 'Unwanted chat in public area'
      }
    });
    expect(reportRes.statusCode).toBe(200);
    expect(reportRes.json().success).toBe(true);
  });

  it('DELETE /api/v1/users/me deletes account for privacy compliance', async () => {
    const delRes = await app.inject({
      method: 'DELETE',
      url: '/api/v1/users/me',
      headers: {
        authorization: 'Bearer dev-user:player_test_1'
      }
    });
    expect(delRes.statusCode).toBe(200);
    expect(delRes.json().success).toBe(true);

    const getRes = await app.inject({
      method: 'GET',
      url: '/api/v1/users/me',
      headers: {
        authorization: 'Bearer dev-user:player_test_1'
      }
    });
    expect(getRes.statusCode).toBe(200);
    expect(getRes.json().data).toBeNull();
  });
});
