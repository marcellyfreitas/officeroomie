import { ReactNode } from 'react';

export interface AuthContextType<T> {
	access_token: string;
	refresh_token: string;
	userData: T | null;
	login: (username: string, password: string) => Promise<any>;
	register: (name: string, email: string, cpf: string, password: string, addressId?: number) => Promise<any>;
	logout: () => Promise<any>;
}

interface AuthProviderProps {
	children: ReactNode;
}

