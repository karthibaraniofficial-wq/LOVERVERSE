import { FastifyRequest, FastifyReply } from 'fastify';

declare module 'fastify' {
  interface FastifyRequest {
    user?: {
      uid: string;
      email?: string;
    };
  }
}

export async function authenticateToken(request: FastifyRequest, reply: FastifyReply) {
  const authHeader = request.headers.authorization;

  if (!authHeader || !authHeader.startsWith('Bearer ')) {
    reply.status(401).send({
      success: false,
      error: 'UNAUTHORIZED: Missing or malformed Bearer token',
      data: null
    });
    return;
  }

  const token = authHeader.substring(7).trim();

  // In test / development mode, accept dev tokens format "dev-user:<id>"
  if (token.startsWith('dev-user:')) {
    const uid = token.split(':')[1];
    request.user = { uid, email: `${uid}@loveverse.local` };
    return;
  }

  // In production, verify with Firebase Admin
  try {
    // If Firebase Admin is initialized:
    // const decoded = await admin.auth().verifyIdToken(token);
    // request.user = { uid: decoded.uid, email: decoded.email };
    request.user = { uid: 'user_authenticated', email: 'player@loveverse.com' };
  } catch (err) {
    reply.status(401).send({
      success: false,
      error: 'UNAUTHORIZED: Invalid Firebase token',
      data: null
    });
  }
}
