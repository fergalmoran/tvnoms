import CryptoES from "crypto-es";

export const decryptData = (text: string, key: string, throwIfError: boolean = true): any => {
  try {
    const decryptedData = CryptoES.AES.decrypt(text, key).toString(CryptoES.enc.Utf8);
    return JSON.parse(decryptedData);
  } catch (error) {
    if (throwIfError) throw error;
    else console.warn("Decryption failed: " + error);
  }
  return null;
};
export const encryptData = (value: any, key: string, throwIfError: boolean = true): string | null => {
  try {
    const encryptedData = CryptoES.AES.encrypt(JSON.stringify(value), key).toString();
    return encryptedData;
  } catch (error) {
    if (throwIfError) throw error;
    else console.warn("Encryption failed: " + error);
  }
  return null;
};
