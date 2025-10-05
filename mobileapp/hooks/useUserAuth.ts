import { useContext } from 'react';
import { UserAuthenticationContext } from '@/contexts/UserAuthenticationContext';

export const useUserAuth = () => useContext(UserAuthenticationContext);