import { useContext } from 'react';
import { UserAuthenticationContext } from '@/contexts/AdminAuthenticationContext';


export const useAdminAuth = () => useContext(UserAuthenticationContext);