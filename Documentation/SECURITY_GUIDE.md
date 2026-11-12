# LOVEVERSE - Security & Privacy Guide

## Principles
1. **Zero Client Trust:** Clients never write directly to world inventories or economy collections.
2. **Private Rooms Cryptography:** 6-character room codes generated via cryptographically secure random values with collision resistance.
3. **Firestore Security Rules:** Strict rules enforcing that only authorized room/world members can access private documents.
4. **No Secrets in Client:** API keys and service account credentials strictly isolated in backend environment variables.
