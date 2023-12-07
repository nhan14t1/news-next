import axios from 'axios';
import { BASE_API_URL } from '../constants/app-const';
import * as STORAGE_KEY from '../constants/storage-key-const';

const getOptions = () => {
  return {
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + localStorage.getItem(STORAGE_KEY.ACCESS_TOKEN_KEY),
    },
    // NODE_TLS_REJECT_UNAUTHORIZED:'0'
  };
}

const getFileOptions = () => {
  return {
    headers: {
      Authorization: 'Bearer ' + localStorage.getItem(STORAGE_KEY.ACCESS_TOKEN_KEY),
    },
    // NODE_TLS_REJECT_UNAUTHORIZED:'0'
  };
}

export const fetch = async (url) => {
  try {
    process.env['NODE_TLS_REJECT_UNAUTHORIZED'] = '0';
    const res = await fetch(BASE_API_URL + url,
      {
        headers: {
          'Content-Type': 'application/json',
          Authorization: 'Bearer ' + localStorage.getItem(STORAGE_KEY.ACCESS_TOKEN_KEY),
        }
      });

    return await res.json();
  } catch (err) {
    return await { error: err.message, isError: true };
  }
}

export const get = (url, isCatchError = true) => {
  return axios.get(BASE_API_URL + url, getOptions())
    .catch((res) => {
      handleError(res.response, isCatchError);
      return res;
    });
}

export const post = (url, data, isCatchError = true) => {
  return axios.post(BASE_API_URL + url, data, getOptions())
    .catch((res) => {
      handleError(res.response, isCatchError);
      return res;
    });;
}

export const put = (url, data, isCatchError = true) => {
  return axios.put(BASE_API_URL + url, data, getOptions())
    .catch((res) => {
      handleError(res.response, isCatchError);
      return res;
    });;
}

export const deleteAPI = (url, isCatchError = true) => {
  return axios.get(BASE_API_URL + url, getOptions())
    .catch((res) => {
      handleError(res.response, isCatchError);
      return res;
    });
}

const handleError = (res, isCatchError = true) => {
  if (res && res.status === 401) {
    // Clear cache
    // Remove old access token if have
    if (removeStoreLoggedUser) {
      removeStoreLoggedUser();
    }

    // warningAlert('Your token has expired', 2500);
    setTimeout(() => {
      // Navigate to login page
      window.location.href = window.location.origin + '/login';
    }, 2000);
    return;
  }

  if (res && res.status === 403) {
    //
    // Navigate to forbidden page
    window.location.href = window.location.origin + '/forbidden';
  }

  let messageError = res && res.data && res.data.message
    ? res.data.message : 'Sorry, an error has occurred';

  if (isCatchError) {
    // errorAlert(messageError);
  } else {
    throw new Error(messageError);
  }
}

const removeStoreLoggedUser = () => {
  localStorage.removeItem(STORAGE_KEY.ACCESS_TOKEN_KEY);
}