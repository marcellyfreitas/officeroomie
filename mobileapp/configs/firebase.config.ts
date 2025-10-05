import { initializeApp } from 'firebase/app';
import { getDatabase } from 'firebase/database';

const firebaseConfig = {
	apiKey: process.env.EXPO_PUBLIC_FIREBASE_API_KEY,
	appId: process.env.EXPO_PUBLIC_FIREBASE_APP_ID,
	authDomain: process.env.EXPO_PUBLIC_FIREBASE_AUTH_DOMAIN,
	projectId: process.env.EXPO_PUBLIC_FIREBASE_PROJECT_ID,
	storageBucket: process.env.EXPO_PUBLIC_FIREBASE_STORAGE_BUCKET,
	messagingSenderId: process.env.EXPO_PUBLIC_FIREBASE_MESSAGING_SENDER_ID,
	databaseURL: process.env.EXPO_PUBLIC_FIREBASE_DATABASE_URL,
};

const app = initializeApp(firebaseConfig);

export const db = getDatabase(app);
