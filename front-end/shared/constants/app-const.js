export const BASE_API_URL = `${process.env.NEXT_PUBLIC_BASE_API_URL}/api`;
export const BASE_IMAGE_URL = `${process.env.NEXT_PUBLIC_BASE_API_URL}/images/posts`;
export const BASE_THUMBNAIL_URL = `${process.env.NEXT_PUBLIC_BASE_API_URL}/images/thumbnails`;
export const WEB_NAME = 'Showbiz 365';
export const BASE_URL = process.env.NEXT_PUBLIC_BASE_URL;
export const ROLES = {
  Admin: { id: 1, Name: 'Admin'},
  User: { id: 2, Name: 'User'},
};

export const CATEGORIES = {
  VietNam: {id: 1, name: 'Showbiz Việt Nam'},
  Global: {id: 2, name: 'Showbiz Quốc tế'},
};

export const POST_STATUS = {
  Active : { id: 1, name: 'Hoạt động'},
  Schedule : { id: 2, name: 'Lên lịch'},
  Draft : { id: 3, name: 'Nháp'},
}

export const TEST_IMAGE_URL = 'assets/images/default.jpg';
export const IMAGE_POST_PREFIX = 'nqim-';