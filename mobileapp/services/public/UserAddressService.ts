import { db } from '@/configs/firebase.config';
import {
	ref,
	set,
	update,
	remove,
	get,
	child,
	query,
	orderByChild,
	equalTo,
	push,
} from 'firebase/database';
import { HttpClient } from '@/@types/IHttpClient';

export class UserAddressService {
	private basePath = 'users_addresses';
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async search(cep: string): Promise<any> {
		const API_URL = process.env.EXPO_PUBLIC_APICEP_URL ?? '';
		const REPLACED = API_URL?.replace('{cep}', cep);
		return this.client.get(REPLACED);
	}

	async addFromUserAsync(data: any): Promise<any> {
		const addressRef = ref(db, this.basePath);
		const newAddressRef = push(addressRef);
		await set(newAddressRef, data);
		return { id: newAddressRef.key, ...data };
	}

	async getAllFromUserAsync(user_id: number): Promise<any[]> {
		const q = query(
			ref(db, this.basePath),
			orderByChild('user_id'),
			equalTo(user_id),
		);

		const snapshot = await get(q);

		if (!snapshot.exists()) return [];

		const data = snapshot.val();

		return Object.entries(data).map(([id, value]) => ({ id, ...(value as any) }));
	}

	async getByIdFromUserAsync(addressId: string): Promise<any> {
		const snapshot = await get(child(ref(db), `${this.basePath}/${addressId}`));
		if (!snapshot.exists()) return null;
		return { id: addressId, ...snapshot.val() };
	}

	async updateFromUserAsync(addressId: string, data: any): Promise<void> {
		await update(ref(db, `${this.basePath}/${addressId}`), data);
	}

	async deleteFromUserAsync(addressId: string): Promise<void> {
		await remove(ref(db, `${this.basePath}/${addressId}`));
	}
}
