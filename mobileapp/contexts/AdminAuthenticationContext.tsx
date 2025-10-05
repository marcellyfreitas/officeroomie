import { createContext, useState, useEffect, useCallback } from 'react';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { AuthContextType, AuthProviderProps, IAdmin } from '@/@types';
import { AdminAuthenticationService } from '@/services/authentication/AdminAuthenticationService';
import { HttpClient } from '@/services/restrict/HttpClient';

type AdminContextType = Omit<AuthContextType<IAdmin>, 'register'>;

const defaultValue: AdminContextType = {
	access_token: '',
	refresh_token: '',
	userData: null,
	login: async () => { },
	logout: async () => { },
};

export const USER_ACCESS_TOKEN_NAME = 'admin_access_token';
const USER_REFRESH_TOKEN_NAME = 'admin_refresh_token';
const USER_DATA_NAME = 'admin_userData';

export const UserAuthenticationContext = createContext<AdminContextType>(defaultValue);

export const AuthProvider = ({ children }: AuthProviderProps) => {
	const [access_token, setAccessToken] = useState<string>(defaultValue.access_token);
	const [refresh_token, setRefreshToken] = useState<string>(defaultValue.refresh_token);
	const [userData, setUserData] = useState<IAdmin | null>(defaultValue.userData);
	const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
	const service = new AdminAuthenticationService(client);

	const loadAuthData = useCallback(async () => {
		const storedAccessToken = await AsyncStorage.getItem(USER_ACCESS_TOKEN_NAME);
		const storedUserData = await AsyncStorage.getItem(USER_DATA_NAME);

		if (storedAccessToken && storedUserData) {
			setAccessToken(storedAccessToken);
			setUserData(JSON.parse(storedUserData));
		}
	}, []);

	useEffect(() => {
		loadAuthData();
	}, [loadAuthData]);

	const login = async (username: string, password: string) => {
		try {
			const { token } = await service.login({ username, password });

			await AsyncStorage.setItem(USER_ACCESS_TOKEN_NAME, token);

			setAccessToken(token);

			const user = await service.userData();


			setUserData(user);

			await AsyncStorage.setItem(USER_DATA_NAME, JSON.stringify(user));

			return { token };
		} catch (error) {
			console.error('Erro ao fazer login:', error);
			throw error;
		}
	};

	const logout = async () => {
		try {
			await AsyncStorage.removeItem(USER_ACCESS_TOKEN_NAME);
			await AsyncStorage.removeItem(USER_REFRESH_TOKEN_NAME);
			await AsyncStorage.removeItem(USER_DATA_NAME);
			setAccessToken('');
			setRefreshToken('');
			setUserData(null);
		} catch (error) {
			console.error('Erro ao fazer logout:', error);
			throw error;
		}
	};

	return (
		<UserAuthenticationContext.Provider
			value={{
				access_token,
				refresh_token,
				userData,
				login,
				logout,
			}}
		>
			{children}
		</UserAuthenticationContext.Provider>
	);
};