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
    // First create room
    const createRes = await app.inject({
      method: 'POST',
      url: '/api/v1/rooms',
      headers: {
        authorization: 'Bearer dev-user:player_test_1'
      }
    });
    const room = createRes.json().data;

    // Join room as player 2
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

  it('GET /api/v1/worlds/world_demo loads shared world state', async () => {
    const res = await app.inject({
      method: 'GET',
      url: '/api/v1/worlds/world_demo',
      headers: {
        authorization: 'Bearer dev-user:player_test_1'
      }
    });
    expect(res.statusCode).toBe(200);
    expect(res.json().data.currentRegion).toBe('LoveTown');
  });
});
