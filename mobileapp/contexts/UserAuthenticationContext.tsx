import { createContext, useState, useEffect } from 'react';
import { UserAuthenticationService } from '@/services/authentication/UserAuthenticationService';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { AuthContextType, AuthProviderProps, IUser } from '@/@types';
import { HttpClient } from '@/services/restrict/HttpClient';

type InnerAuthContextType = AuthContextType<IUser> & { updateUserData: (data: any) => void };

const defaultValue: InnerAuthContextType = {
	access_token: '',
	refresh_token: '',
	userData: null,
	login: async () => { },
	register: async () => { },
	logout: async () => { },
	updateUserData: () => { },
};

export const USER_ACCESS_TOKEN_NAME = 'user_access_token';
const USER_REFRESH_TOKEN_NAME = 'user_refresh_token';
const USER_DATA_NAME = 'user_userData';

export const UserAuthenticationContext = createContext<InnerAuthContextType>(defaultValue);

export const AuthProvider = ({ children }: AuthProviderProps) => {
	const [access_token, setAccessToken] = useState<string>(defaultValue.access_token);
	const [refresh_token, setRefreshToken] = useState<string>(defaultValue.refresh_token);
	const [userData, setUserData] = useState<IUser | null>(defaultValue.userData);
	const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
	const service = new UserAuthenticationService(client);

	useEffect(() => {
		const loadAuthData = async () => {
			const storedAccessToken = await AsyncStorage.getItem(USER_ACCESS_TOKEN_NAME);
			const storedUserData = await AsyncStorage.getItem(USER_DATA_NAME);

			if (storedAccessToken && storedUserData) {
				setAccessToken(storedAccessToken);
				setUserData(JSON.parse(storedUserData));
			}
		};

		loadAuthData();
	}, []);

	const login = async (username: string, password: string) => {
		try {
			const { token } = await service.login({ username, password });

			await AsyncStorage.setItem(USER_ACCESS_TOKEN_NAME, token);

			setAccessToken(token);

			const user = await service.userData();

			await updateUserData(user);

			return { token };
		} catch (error) {
			console.error('Erro ao fazer login:', error);
			throw error;
		}
	};

	const register = async (name: string, email: string, cpf: string, password: string, addressId?: number) => {
		try {
			const { token } = await service.register({ name, email, cpf, password, addressId });

			await AsyncStorage.setItem(USER_ACCESS_TOKEN_NAME, token);

			setAccessToken(token);

			const user = await service.userData();

			await updateUserData(user);

			return { token };
		} catch (error) {
			console.error('Erro ao fazer login:', error);
			throw error;
		}
	};

	const logout = async () => {
		try {
			setAccessToken('');
			setRefreshToken('');
			setUserData(null);

			await AsyncStorage.removeItem(USER_ACCESS_TOKEN_NAME);
			await AsyncStorage.removeItem(USER_REFRESH_TOKEN_NAME);
			await AsyncStorage.removeItem(USER_DATA_NAME);
		} catch (error) {
			console.error('Erro ao fazer logout:', error);
			throw error;
		}
	};

	const updateUserData = async (user: IUser) => {
		await AsyncStorage.setItem(USER_DATA_NAME, JSON.stringify(user));
		setUserData(user);
	};

	return (
		<UserAuthenticationContext.Provider
			value={{
				access_token,
				refresh_token,
				userData,
				register,
				login,
				logout,
				updateUserData,
			}}
		>
			{children}
		</UserAuthenticationContext.Provider>
	);
};