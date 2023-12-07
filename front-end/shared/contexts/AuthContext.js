import axios from "axios";
import { createContext, useState } from "react";
import { jwtDecode } from "jwt-decode";
import { useRouter } from 'next/router';
import { ACCESS_TOKEN_KEY } from "../constants/storage-key-const";

const AuthContext = createContext();

export const AuthContextProvider = ({ children }) => {
  if (typeof window === 'undefined') {
    return <></>;
  }

  const [user, setUser] = useState(() => {
    if (localStorage.getItem(ACCESS_TOKEN_KEY)) {
      return jwtDecode(localStorage.getItem(ACCESS_TOKEN_KEY));
    }
    return null;
  });
 
  const router = useRouter();
 
  const login = async (payload) => {
    const apiResponse = await axios.post(
      "http://localhost:3000/login",
      payload
    );
    localStorage.setItem(ACCESS_TOKEN_KEY,  JSON.stringify(apiResponse.data));
    setUser(jwt_decode(apiResponse.data.access_token));
    router.push('/login');
  };
  
  return (
    <AuthContext.Provider value={{ user, login }}>
      {children}
    </AuthContext.Provider>
  );
};
 
export default AuthContext;