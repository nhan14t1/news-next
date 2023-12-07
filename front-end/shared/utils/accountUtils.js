import * as StorageKey from '../constants/storage-key-const';

export const saveInfo = (user) => {
  localStorage.setItem(StorageKey.USER_ID, user.id);
  localStorage.setItem(StorageKey.USER_NAME, user.userName);
  localStorage.setItem(StorageKey.ACCESS_TOKEN_KEY, user.accessToken);
  localStorage.setItem(StorageKey.ACCESS_TOKEN_EXPIRATION_TIMESTAMP, user.tokenExpirationInTimeStamp.toString());
  localStorage.setItem(StorageKey.FIRST_NAME, user.firstName || '');
  localStorage.setItem(StorageKey.LAST_NAME, user.lastName || '');
  localStorage.setItem(StorageKey.AVATAR_URL, user.avatarUrl || '');
}